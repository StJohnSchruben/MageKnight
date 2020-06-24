

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.ServiceModel;
using System.Text;
using log4net;
using ReDefNet;
using Service;
using Service.Contract;

namespace ServerHost
{
    /// <summary>
    /// A hosting server for the services infrastructure. This class cannot be inherited.
    /// </summary>
    public sealed class Server : IServer
    {
        /// <summary>
        /// The name of the file used to read the name of the snapshot that should be loaded upon startup.
        /// </summary>
        private const string snapshotLoadFileName = "SelectedICSetPath.txt";

        /// <summary>
        /// The command handlers.
        /// </summary>
        private readonly List<object> commandHandlers = new List<object>();

        /// <summary>
        /// The device bootstrappers.
        /// </summary>
        private readonly List<IDeviceServiceBootstrapper> deviceBootstrappers = new List<IDeviceServiceBootstrapper>();

        /// <summary>
        /// The known type assemblies.
        /// </summary>
        private readonly string[] knownTypeAssemblies;

        /// <summary>
        /// The log.
        /// </summary>
        private readonly ILog log = LogManager.GetLogger(typeof(Server));

        /// <summary>
        /// The thread-synchronizing object.
        /// </summary>
        private readonly object padLock = new object();

        /// <summary>
        /// The query handlers.
        /// </summary>
        private readonly List<IQueryHandler> queryHandlers = new List<IQueryHandler>();

        /// <summary>
        /// The service endpoint.
        /// </summary>
        private readonly IPEndPoint serviceEndPoint;

        /// <summary>
        /// The special device service bootstrapper that turns snapshot-related command messages into events.
        /// </summary>
        private readonly ISnapshotAdapterDeviceServiceBootstrapper snapshotAdapterDeviceServiceBootstrapper;

        /// <summary>
        /// The command service.
        /// </summary>
        private CommandService commandService;

        /// <summary>
        /// The command service host.
        /// </summary>
        private ServiceHost commandServiceHost;

        /// <summary>
        /// The query service.
        /// </summary>
        private QueryService queryService;

        /// <summary>
        /// The query service host.
        /// </summary>
        private ServiceHost queryServiceHost;

        /// <summary>
        /// Initializes a new instance of the <see cref="Server" /> class.
        /// </summary>
        public Server()
            : this(null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Server" /> class.
        /// </summary>
        /// <param name="serviceEndPoint">The service endpoint.</param>
        public Server(IPEndPoint serviceEndPoint)
        {
            this.State = CommunicationState.Closed;

            this.log.Info("Initializing Block II server.");

            this.serviceEndPoint = serviceEndPoint;

            var executingDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            this.log.DebugFormat(
                "Scanning '{0}' for assemblies containing device service bootstrappers.",
                executingDirectory);

            // ReSharper disable once AssignNullToNotNullAttribute
            var assemblyPaths = Directory.GetFiles(executingDirectory, "*.dll");

            var tempKnownTypeAssemblies = new List<string>();

            foreach (var assemblyPath in assemblyPaths)
            {
                this.log.DebugFormat("Attempting to load assembly: {0}.", assemblyPath);

                Assembly assembly;
                try
                {
                    assembly = Assembly.LoadFrom(assemblyPath);
                }
                catch (Exception e)
                {
                    this.log.Warn($"Loading assembly '{assemblyPath}' threw an exception.", e);

                    continue;
                }

                var bootstrapperTypes = from type in assembly.GetExportedTypes()
                                        where type.IsPublic &&
                                            type.IsClass &&
                                            !type.IsAbstract &&
                                            type.GetInterfaces().Contains(typeof(IDeviceServiceBootstrapper))
                                        select type;

                this.log.Debug("Made it past bootstrapperTypes.");
                foreach (var type in bootstrapperTypes)
                {
                    this.log.DebugFormat("Creating instance of device adapter, '{0}'.", type.FullName);

                    IDeviceServiceBootstrapper bootstrapper;
                    try
                    {
                        bootstrapper = (IDeviceServiceBootstrapper)Activator.CreateInstance(type);
                    }
                    catch (Exception e)
                    {
                        this.log.Warn(
                            $"Creating instance of device adapter, '{type.FullName}', threw an exception.",
                            e);

                        continue;
                    }

                    this.deviceBootstrappers.Add(bootstrapper);
                }
            }

            foreach (var deviceBootstrapper in this.deviceBootstrappers)
            {
                this.log.DebugFormat(
                    "Reading known type assemblies from device service bootstrapper '{0}'.",
                    deviceBootstrapper.GetType().AssemblyQualifiedName);

                tempKnownTypeAssemblies.AddRange(deviceBootstrapper.KnownTypeAssemblies);
            }

            this.log.DebugFormat("Attempting to find the snapshot adapter device service bootstrapper.");

            foreach (var deviceBootstrapper in this.deviceBootstrappers)
            {
                if (!(deviceBootstrapper is ISnapshotAdapterDeviceServiceBootstrapper))
                {
                    continue;
                }

                this.log.DebugFormat(
                    "Found snapshot adapter device service bootstrapper '{0}'.",
                    deviceBootstrapper.GetType().AssemblyQualifiedName);
                this.log.DebugFormat("The service WILL participate in snapshot.");

                this.snapshotAdapterDeviceServiceBootstrapper =
                    (ISnapshotAdapterDeviceServiceBootstrapper)deviceBootstrapper;

                this.snapshotAdapterDeviceServiceBootstrapper.CreateSnapshotRequested +=
                    this.OnCreateSnapshotRequested;

                break;
            }

            if (this.snapshotAdapterDeviceServiceBootstrapper == null)
            {
                this.log.Debug("No snapshot adapter device service bootstrapper was found.");
                this.log.Debug("The service will NOT participate in snapshot.");
            }

            this.knownTypeAssemblies = tempKnownTypeAssemblies.Distinct().ToArray();

            this.log.DebugFormat(
                "Finished scanning for device service bootstrappers. Known type assemblies are:\r\n\t{0}.",
                string.Join("\r\n\t", this.knownTypeAssemblies));

            this.log.Info("Block II server initialized successfully.");
        }

        /// <summary>
        /// Occurs when the server is closed.
        /// </summary>
        public event EventHandler Closed;

        /// <summary>
        /// Occurs when the server is faulted.
        /// </summary>
        public event EventHandler Faulted;

        /// <summary>
        /// Occurs when the server is opened.
        /// </summary>
        public event EventHandler Opened;

        /// <summary>
        /// Gets a value indicating whether the server is open.
        /// </summary>
        /// <value><c>true</c>, if the server is open; otherwise, <c>false</c>.</value>
        public bool IsOpen
        {
            get
            {
                return this.State == CommunicationState.Opened;
            }
        }

        /// <summary>
        /// Gets the current server state.
        /// </summary>
        /// <value>
        /// The server state.
        /// </value>
        public CommunicationState State { get; private set; }

        /// <summary>
        /// Closes the server.
        /// </summary>
        public void Close()
        {
            lock (this.padLock)
            {
                if (this.State == CommunicationState.Closing ||
                    this.State == CommunicationState.Closed)
                {
                    return;
                }

                this.ChangeState(CommunicationState.Closing);
            }

            this.log.Info("Preparing to close Block II service hosts.");

            this.PerformClose();

            lock (this.padLock)
            {
                this.ChangeState(CommunicationState.Closed);
            }

            this.log.Info("Block II service hosts closed successfully.");
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this.Close();
        }

        /// <summary>
        /// Opens the server.
        /// </summary>
        public void Open()
        {
            lock (this.padLock)
            {
                if (this.State == CommunicationState.Opened ||
                    this.State == CommunicationState.Opening)
                {
                    return;
                }

                this.ChangeState(CommunicationState.Opening);
            }

            this.log.Info("Preparing to open Block II service hosts.");

            if (this.serviceEndPoint == null)
            {
                this.log.Debug("Service configuration will be read from configuration file.");
            }
            else
            {
                this.log.DebugFormat(
                    "Service configuration will be hard-coded using endpoint {0}.",
                    this.serviceEndPoint);
            }

            this.log.Debug("Creating service hosts.");

            try
            {
                if (this.serviceEndPoint == null)
                {
                    this.CreateConfigFileCommandServiceHost();
                    this.CreateConfigFileQueryServiceHost();
                }
                else
                {
                    this.CreateHardCodedCommandServiceHost();
                    this.CreateHardCodedQueryServiceHost();
                }
            }
            catch (Exception e)
            {
                this.log.Error("Creating service hosts threw an exception.", e);

                lock (this.padLock)
                {
                    this.ChangeState(CommunicationState.Faulted);
                }

                return;
            }

            this.log.Debug("Loading snapshot (if needed).");

            try
            {
                this.LoadSnapshot();
            }
            catch (Exception e)
            {
                this.log.Error("Loading snapshot threw an exception.", e);

                this.PerformClose();

                lock (this.padLock)
                {
                    this.ChangeState(CommunicationState.Faulted);
                }

                return;
            }

            this.log.Debug("Loading query and service handlers.");

            try
            {
                foreach (var deviceBootstrapper in this.deviceBootstrappers)
                {
                    this.queryHandlers.AddRange(
                        deviceBootstrapper.GetQueryHandlers(
                            this.queryService,
                            this.commandService,
                            this.queryService));

                    this.commandHandlers.AddRange(deviceBootstrapper.GetCommandHandlers(this.commandService));
                }
            }
            catch (Exception e)
            {
                this.log.Error("Loading query and service handlers threw an exception.", e);

                this.PerformClose();

                lock (this.padLock)
                {
                    this.ChangeState(CommunicationState.Faulted);
                }

                return;
            }

            this.log.DebugFormat("Opening command service host.");

            try
            {
                this.commandServiceHost.Open();
            }
            catch (Exception e)
            {
                this.log.Error("Opening command service host threw an exception.", e);

                this.PerformClose();

                lock (this.padLock)
                {
                    this.ChangeState(CommunicationState.Faulted);
                }

                return;
            }

            this.log.DebugFormat("Opening query service host.");

            try
            {
                this.queryServiceHost.Open();
            }
            catch (Exception e)
            {
                this.log.Error("Opening query service host threw an exception.", e);

                this.PerformClose();

                lock (this.padLock)
                {
                    this.ChangeState(CommunicationState.Faulted);
                }

                return;
            }

            lock (this.padLock)
            {
                this.ChangeState(CommunicationState.Opened);
            }

            this.log.Info("Block II service hosts opened successfully.");
        }

        /// <summary>
        /// Changes the current state of the server and raises the appropriate events accordingly.
        /// </summary>
        /// <param name="updatedState">The new state to change to.</param>
        private void ChangeState(CommunicationState updatedState)
        {
            var previousState = this.State;

            if (previousState == updatedState)
            {
                return;
            }

            this.State = updatedState;

            switch (updatedState)
            {
                case CommunicationState.Closed:
                {
                    this.Closed?.RaiseEvent(EventArgs.Empty);

                    break;
                }

                case CommunicationState.Opened:
                {
                    this.Opened?.RaiseEvent(EventArgs.Empty);

                    break;
                }

                case CommunicationState.Faulted:
                {
                    this.Faulted?.RaiseEvent(EventArgs.Empty);

                    break;
                }
            }
        }

        /// <summary>
        /// Converts the UNC snapshot path received from the snapshot-related events to a local path.
        /// </summary>
        /// <param name="path">The UNC snapshot path.</param>
        /// <returns>
        /// The local snapshot path.
        /// </returns>
        private string ConvertSnapshotPath(string path)
        {
            this.log.DebugFormat("Converting UNC snapshot path '{0}' to local path.", path);

            if (string.IsNullOrEmpty(path))
            {
                this.log.Error("UNC snapshot path is empty.");

                return null;
            }

            var index = path.IndexOf("ICSets", StringComparison.Ordinal);

            if (index < 0)
            {
                this.log.ErrorFormat("UNC snapshot path '{0}' is invalid.", path);

                return null;
            }

            /*
             * Typical UNC path is something like:
             *      \\192.168.17.95\ICSets\PersistentICs\PreFlightICs\SNAPSHOT_NAME\BlockII      <-- Block II model snapshot files
             *      \\192.168.17.95\ICSets\PersistentICs\PreFlightICs\SNAPSHOT_NAME\OAT          <-- Devices/Windows indiviual snapshot files
             *      \\192.168.17.95\ICSets\PersistentICs\PreFlightICs\SNAPSHOT_NAME\ScenarioData <-- Current runtime scenario directory from IOS
             */
            path = path.Substring(index);

            if (!path.EndsWith("\\"))
            {
                path += "\\";
            }

            path = "C:\\" + path + "BlockII\\";

            this.log.DebugFormat("Local snapshot path is '{0}'.", path);

            return path;
        }

        /// <summary>
        /// Creates the command service host using the WCF configuration from the application configuration file.
        /// </summary>
        private void CreateConfigFileCommandServiceHost()
        {
            this.log.Debug("Creating command service host using settings from application configuration file.");

            this.commandService = new CommandService();
            this.commandServiceHost = new ServiceHost(this.commandService);
            this.commandServiceHost.Faulted += this.OnCommandServiceHostFaulted;

            foreach (var endpoint in this.commandServiceHost.Description.Endpoints)
            {
                endpoint.Contract.ContractBehaviors.Add(new KnownTypeContractBehavior(this.knownTypeAssemblies));

                endpoint.Behaviors.Add(new MessageLoggingEndpointBehavior());
                endpoint.Behaviors.Add(new SerializationLoggingEndpointBehavior());
            }
        }

        /// <summary>
        /// Creates the query service host using the WCF configuration from the application configuration file.
        /// </summary>
        private void CreateConfigFileQueryServiceHost()
        {
            this.log.Debug("Creating query service host using settings from application configuration file.");

            this.queryService = new QueryService();
            this.queryServiceHost = new ServiceHost(this.queryService);
            this.queryServiceHost.Faulted += this.OnQueryServiceHostFaulted;

            foreach (var endpoint in this.queryServiceHost.Description.Endpoints)
            {
                endpoint.Contract.ContractBehaviors.Add(new KnownTypeContractBehavior(this.knownTypeAssemblies));

                endpoint.Behaviors.Add(new MessageLoggingEndpointBehavior());
                endpoint.Behaviors.Add(new SerializationLoggingEndpointBehavior());
            }
        }

        /// <summary>
        /// Creates the command service host using a hard-coded WCF configuration.
        /// </summary>
        private void CreateHardCodedCommandServiceHost()
        {
            this.log.Debug("Creating command service host using hard-coded settings.");

            var binding = new NetTcpBinding
            {
                Name = "NetTcpBinding_ICommandContract",
                ReceiveTimeout = TimeSpan.MaxValue,
                SendTimeout = TimeSpan.MaxValue,
                OpenTimeout = TimeSpan.MaxValue,
                MaxReceivedMessageSize = int.MaxValue,
                MaxBufferSize = int.MaxValue,
                MaxBufferPoolSize = int.MaxValue,
            };

            var uriBuilder = new UriBuilder
            {
                Host = this.serviceEndPoint.Address.ToString(),
                Path = "/Lwi/Wst/Service/Command",
                Port = this.serviceEndPoint.Port,
                Scheme = "net.tcp"
            };

            this.commandService = new CommandService();
            this.commandServiceHost = new ServiceHost(this.commandService, uriBuilder.Uri);
            this.commandServiceHost.AddServiceEndpoint(typeof(ICommandContract), binding, uriBuilder.Uri);
            this.commandServiceHost.Faulted += this.OnCommandServiceHostFaulted;

            var serviceBehavior = this.commandServiceHost.Description.Behaviors.Find<ServiceBehaviorAttribute>();
            serviceBehavior.MaxItemsInObjectGraph = int.MaxValue;

            foreach (var endpoint in this.commandServiceHost.Description.Endpoints)
            {
                endpoint.Contract.ContractBehaviors.Add(new KnownTypeContractBehavior(this.knownTypeAssemblies));
                endpoint.Behaviors.Add(new MessageLoggingEndpointBehavior());
                endpoint.Behaviors.Add(new SerializationLoggingEndpointBehavior());
            }
        }

        /// <summary>
        /// Creates the query service host using a hard-coded WCF configuration.
        /// </summary>
        private void CreateHardCodedQueryServiceHost()
        {
            this.log.Debug("Creating query service host using hard-coded settings.");

            var binding = new NetTcpBinding
            {
                Name = "NetTcpBinding_IQueryContract",
                ReceiveTimeout = TimeSpan.MaxValue,
                SendTimeout = TimeSpan.MaxValue,
                OpenTimeout = TimeSpan.MaxValue,
                MaxReceivedMessageSize = int.MaxValue,
                MaxBufferSize = int.MaxValue,
                MaxBufferPoolSize = int.MaxValue,
            };

            var uriBuilder = new UriBuilder
            {
                Host = this.serviceEndPoint.Address.ToString(),
                Path = "/Lwi/Wst/Service/Query",
                Port = this.serviceEndPoint.Port,
                Scheme = "net.tcp"
            };

            this.queryService = new QueryService();
            this.queryServiceHost = new ServiceHost(this.queryService, uriBuilder.Uri);
            this.queryServiceHost.AddServiceEndpoint(typeof(IQueryContract), binding, uriBuilder.Uri);
            this.queryServiceHost.Faulted += this.OnQueryServiceHostFaulted;

            var serviceBehavior = this.queryServiceHost.Description.Behaviors.Find<ServiceBehaviorAttribute>();
            serviceBehavior.MaxItemsInObjectGraph = int.MaxValue;

            foreach (var endpoint in this.queryServiceHost.Description.Endpoints)
            {
                endpoint.Contract.ContractBehaviors.Add(new KnownTypeContractBehavior(this.knownTypeAssemblies));

                endpoint.Behaviors.Add(new MessageLoggingEndpointBehavior());
                endpoint.Behaviors.Add(new SerializationLoggingEndpointBehavior());
            }
        }

        /// <summary>
        /// Loads a snapshot, if requested, into the device service bootstrappers.
        /// </summary>
        private void LoadSnapshot()
        {
            this.log.Debug("Loading Block II model snapshots.");

            var executingDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var snapshotLoadFileNamePath = Path.Combine(executingDirectory, snapshotLoadFileName);

            this.log.DebugFormat("Checking for presence of snapshot load file at '{0}'.", snapshotLoadFileNamePath);

            if (!File.Exists(snapshotLoadFileNamePath))
            {
                this.log.DebugFormat("Snapshot load file '{0}' does not exist. Skipping loading snapshots.",
                    snapshotLoadFileNamePath);

                return;
            }

            this.log.Debug($"Snapshot load file exists. Path: {snapshotLoadFileNamePath}");
            this.log.Debug("Reading snapshot load file.");

            string[] lines;
            try
            {
                lines = File.ReadAllLines(snapshotLoadFileNamePath);
            }
            catch (Exception e)
            {
                this.log.Error($"Reading snapshot load file '{snapshotLoadFileNamePath}' threw an exception.", e);

                return;
            }

            if (lines.Length == 0 || string.IsNullOrWhiteSpace(lines[0]))
            {
                this.log.DebugFormat("Entire snapshot load file or the first line is empty. Skipping loading snapshots.");

                return;
            }

            if (lines[0].Trim() == "N/A")
            {
                this.log.Debug("Snapshot load file does not contain a path to a ICSet to load. Skipping loading snapshots.");
                return;
            }

            var path = this.ConvertSnapshotPath(lines[0]);

            if (path == null)
            {
                return;
            }

            this.log.DebugFormat("Loading block II model snapshots from path '{0}'.", path);

            if (!Directory.Exists(path))
            {
                this.log.ErrorFormat("Snapshot path '{0}' does not exist. Skipping loading snapshots.", path);

                return;
            }

            string[] snapshotFiles;
            try
            {
                snapshotFiles = Directory.GetFiles(path, "*.xml");
            }
            catch (Exception ex)
            {
                this.log.Error($"Getting the list of snapshot files from path '{path}' threw an exception.", ex);

                return;
            }

            foreach (var snapshotFile in snapshotFiles)
            {
                this.log.DebugFormat("Processing snapshot file '{0}'.", snapshotFile);

                var index = snapshotFile.IndexOf("BlockII", StringComparison.OrdinalIgnoreCase);

                if (index < 0)
                {
                    this.log.ErrorFormat("Snapshot file '{0}' does not have the expected path.", snapshotFile);

                    continue;
                }

                var bootstrapperName = snapshotFile.Substring(index);
                bootstrapperName = bootstrapperName.Replace("BlockII", string.Empty);
                bootstrapperName = bootstrapperName.TrimStart('\\');
                bootstrapperName = bootstrapperName.Replace(".xml", string.Empty);

                this.log.DebugFormat(
                    "Attempting to locate device service bootstrapper with full name '{0}'.",
                    bootstrapperName);

                var bootstrapper = this.deviceBootstrappers.SingleOrDefault(x => x.GetType().FullName == bootstrapperName);

                if (bootstrapper == null)
                {
                    this.log.ErrorFormat(
                        "Could not locate a device service bootstrapper with full name '{0}'. Ignoring snapshot file '{1}'.",
                        bootstrapperName,
                        snapshotFile);

                    continue;
                }

                if (!(bootstrapper is IDeviceServiceBootstrapperWithSnapshot))
                {
                    this.log.ErrorFormat(
                        "Device service bootstrapper with full name '{0}' does not support snapshot. Ignoring snapshot file '{1}'.",
                        bootstrapperName,
                        snapshotFile);

                    continue;
                }

                var snapshotBootstrapper = (IDeviceServiceBootstrapperWithSnapshot)bootstrapper;

                string snapshot;
                try
                {
                    this.log.DebugFormat(
                        "Reading contents of snapshot from file '{0}'.",
                        snapshotFile);

                    snapshot = File.ReadAllText(snapshotFile, Encoding.UTF8);

                    this.log.DebugFormat(
                        "Successfully read {0} characters from snapshot file '{1}'.",
                        snapshot.Length,
                        snapshotFile);
                }
                catch (Exception ex)
                {
                    this.log.Error(
                        $"Reading snapshot file '{snapshotFile}' threw an exception.",
                        ex);

                    continue;
                }

                try
                {
                    this.log.DebugFormat(
                        "Telling device service bootstrapper '{0}' to load snapshot.",
                        bootstrapperName);

                    snapshotBootstrapper.LoadSnapshot(snapshot);

                    this.log.DebugFormat(
                        "Device service bootstrapper '{0}' successfully loaded snapshot.",
                        bootstrapperName);
                }
                catch (Exception ex)
                {
                    this.log.Error(
                        $"Telling device service bootstrapper '{bootstrapperName}' to load snapshot threw an exception.",
                        ex);
                }
            }

            this.log.DebugFormat("Block II model snapshots loaded successfully.");
        }

        /// <summary>
        /// Called when the command service is faulted.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event data.</param>
        private void OnCommandServiceHostFaulted(object sender, EventArgs e)
        {
            this.log.Error("The command service has faulted. The service hosts will shut down now.");

            lock (this.padLock)
            {
                this.ChangeState(CommunicationState.Faulted);
            }

            this.PerformClose();
        }

        /// <summary>
        /// Called when the a request to create a snapshot has been received.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event data.</param>
        private void OnCreateSnapshotRequested(object sender, SnapshotEventArgs e)
        {
            this.log.DebugFormat("Received a request to create a snapshot at path '{0}'.", e.Path);

            var path = this.ConvertSnapshotPath(e.Path);

            if (path == null)
            {
                return;
            }

            this.log.DebugFormat("Creating block II model snapshots at path '{0}'.", path);

            try
            {
                this.log.DebugFormat("Attempting to create snapshot directory path '{0}' if it doesn't exist.", path);

                Directory.CreateDirectory(path);
            }
            catch (Exception ex)
            {
                this.log.Error($"Creating directory path '{path}' threw an exception.", ex);

                return;
            }

            foreach (var deviceBootstrapper in this.deviceBootstrappers)
            {
                var bootstrapperName = deviceBootstrapper.GetType().FullName;

                if (!(deviceBootstrapper is IDeviceServiceBootstrapperWithSnapshot))
                {
                    this.log.DebugFormat(
                        "Skipping device service bootstrapper '{0}' because it does not support snapshots.",
                        bootstrapperName);

                    continue;
                }

                var snapshotDeviceBootstrapper = (IDeviceServiceBootstrapperWithSnapshot)deviceBootstrapper;

                string snapshot;
                try
                {
                    this.log.DebugFormat(
                        "Getting snapshot from device service bootstrapper '{0}'.",
                        bootstrapperName);

                    snapshot = snapshotDeviceBootstrapper.GetSnapshot();

                    this.log.DebugFormat(
                        "Successfully received a snapshot from device service bootstrapper '{0}'.",
                        bootstrapperName);
                }
                catch (Exception ex)
                {
                    this.log.Error(
                        $"Getting snapshot from device service bootstrapper '{bootstrapperName}' threw an exception.",
                        ex);

                    continue;
                }

                if (string.IsNullOrEmpty(snapshot))
                {
                    this.log.DebugFormat(
                        "Not saving snapshot from device service bootstrapper '{0}' because it did not contain any data.",
                        bootstrapperName);

                    continue;
                }

                var snapshotFile = path + bootstrapperName + ".xml";

                try
                {
                    this.log.DebugFormat(
                        "Saving snapshot from device service bootstrapper '{0}' to file '{1}'.",
                        bootstrapperName,
                        snapshotFile);

                    File.WriteAllText(snapshotFile, snapshot, Encoding.UTF8);

                    this.log.DebugFormat(
                        "Successfully saved snapshot from device service bootstrapper '{0}'.",
                        bootstrapperName);
                }
                catch (Exception ex)
                {
                    this.log.Error(
                        $"Saving snapshot from device service bootstrapper '{bootstrapperName}' to file '{snapshotFile}' threw an exception.",
                        ex);
                }
            }

            this.log.DebugFormat("Block II model snapshots created successfully.");
        }

        /// <summary>
        /// Called when the query service is faulted.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event data.</param>
        private void OnQueryServiceHostFaulted(object sender, EventArgs e)
        {
            this.log.Error("The query service has faulted. The service hosts will shut down now.");

            lock (this.padLock)
            {
                this.ChangeState(CommunicationState.Faulted);
            }

            this.PerformClose();
        }

        /// <summary>
        /// Closes the server.
        /// </summary>
        private void PerformClose()
        {
            this.log.DebugFormat("Closing command service host.");

            if (this.commandServiceHost != null)
            {
                this.commandServiceHost.Faulted -= this.OnCommandServiceHostFaulted;
            }

            try
            {
                this.commandServiceHost?.Abort();
            }
            catch
            {
                // swallow!
            }
            finally
            {
                this.commandServiceHost = null;
            }

            this.log.DebugFormat("Closing query service host.");

            if (this.queryServiceHost != null)
            {
                this.queryServiceHost.Faulted -= this.OnQueryServiceHostFaulted;
            }

            try
            {
                this.queryServiceHost?.Abort();
            }
            catch
            {
                // swallow!
            }
            finally
            {
                this.queryServiceHost = null;
            }

            this.queryHandlers.Clear();
            this.commandHandlers.Clear();
        }
    }
}