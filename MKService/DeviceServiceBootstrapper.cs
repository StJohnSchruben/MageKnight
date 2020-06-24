using log4net;
using MKService.Queries;
using Service;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace MKService
{
    public sealed class DeviceServiceBootstrapper : IDeviceServiceBootstrapperWithSnapshot
    {
        /// <summary>
        /// The log.
        /// </summary>
        private readonly ILog log = LogManager.GetLogger(typeof(DeviceServiceBootstrapper));

        /// <summary>
        /// The known query types of GameService.
        /// </summary>
        private readonly List<Type> queryTypes = new List<Type>
        {
            typeof(SessionTimeQuery),
            typeof(MageKnightQuery),
            typeof(UserQuery),
            typeof(UserCollectionQuery),
            typeof(GameQuery),
            typeof(GamesQuery),
            typeof(ClickQuery),
            typeof(DialQuery),
            typeof(StatQuery),
        };

        /// <summary>
        /// The query handlers.
        /// </summary>
        private IList<IQueryHandler> queryHandlers;

        /// <summary>
        /// The server service type provider.
        /// </summary>
        private IServerServiceTypeProvider serverServiceTypeProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="DeviceServiceBootstrapper"/> class.
        /// DO NOT REMOVE! CALLED FROM SERVERHOST! DUE TO THE OVERLOADED CONSTRUCTOR BELOW
        /// AN EXPLICIT DEFAULT CONSTRUCTOR IS NEEDED. THE IMPLICIT CONSTRUCTOR IS NOT AVAILABLE
        /// BECUASE OF THE OVERLOADED CONSTRUCTOR.
        /// </summary>
        public DeviceServiceBootstrapper()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DeviceServiceBootstrapper" /> class.
        /// </summary>
        /// <param name="serverServiceTypeProvider">The server service type provider.</param>
        internal DeviceServiceBootstrapper(IServerServiceTypeProvider serverServiceTypeProvider)
        {
            this.serverServiceTypeProvider = serverServiceTypeProvider;
        }

        /// <summary>
        /// Gets the known type assemblies.
        /// </summary>
        /// <value>The known type assemblies.</value>
        public string[] KnownTypeAssemblies => new[]
        {
            "MKModel",
            "MKService",
        };

        /// <summary>
        /// Gets the command handlers.
        /// </summary>
        /// <param name="commandContract">The command contract.</param>
        /// <returns>An array of objects.</returns>
        public object[] GetCommandHandlers(ICommandContract commandContract)
        {
            return new object[0];
        }

        /// <summary>
        /// Gets the query handlers.
        /// </summary>
        /// <param name="queryContract">The query contract.</param>
        /// <param name="commandContract">The command contract.</param>
        /// <param name="rootHandler">The root handler.</param>
        /// <returns>An array of query handlers.</returns>
        public IQueryHandler[] GetQueryHandlers(
            IQueryContract queryContract,
            ICommandContract commandContract,
            IQueryHandler rootHandler)
        {
            if (this.queryHandlers != null)
            {
                return this.queryHandlers.ToArray();
            }

            if (this.serverServiceTypeProvider == null)
            {
                this.serverServiceTypeProvider = ServiceTypeProvider.Instance;
            }

            var queryHandlerCollectionFactory = this.serverServiceTypeProvider.QueryHandlerCollectionFactory;

            this.queryHandlers = queryHandlerCollectionFactory.Create(
                queryContract,
                commandContract,
                rootHandler);

            return this.queryHandlers.ToArray();
        }

        /// <summary>
        /// Gets a snapshot for the model that this bootstrapper manages.
        /// </summary>
        /// <returns>The snapshot data as a string.</returns>
        public string GetSnapshot()
        {
            this.log.Debug($"Getting snapshot of {this.queryTypes.Count} queries.");

            var snapshotStream = new MemoryStream();
            var snapshotStreamWriter = this.GetXmlWriter(ref snapshotStream, true);

            foreach (var queryType in this.queryTypes)
            {
                try
                {
                    var query = this.queryHandlers[0].Handle(queryType.FullName, null);

                    if (query == null)
                    {
                        this.log.Error($"Getting {queryType.Name} returned null.");
                        continue;
                    }

                    var serializer = new DataContractSerializer(
                        queryType,
                        new DataContractSerializerSettings
                        {
                            DataContractResolver = new LwiDataContractResolver(this.KnownTypeAssemblies)
                        });

                    serializer.WriteObject(snapshotStreamWriter, query);

                    snapshotStreamWriter.Flush();

                    this.log.Debug($"Successfully obtained a snapshot of {queryType.Name}.");
                }
                catch (Exception e)
                {
                    this.log.Error($"Getting snapshot of {queryType.Name} threw an exception.", e);
                }
            }

            this.log.Info("Successfully created MKService's snapshot");

            snapshotStream.Position = 0;
            return $"<{this.GetType()}>" + new StreamReader(snapshotStream).ReadToEnd() + $"</{this.GetType()}>";
        }

        /// <summary>
        /// Loads a snapshot of the model that this bootstrapper manages.
        /// </summary>
        /// <param name="snapshot">The snapshot data as a string.</param>
        public void LoadSnapshot(string snapshot)
        {
            var queryResponses = new List<IQueryResponse>();
            this.log.Info("Loading MKService's snapshot.");
            try
            {
                var xDocument = XDocument.Parse(snapshot);

                foreach (var queryType in this.queryTypes)
                {
                    XNamespace ns = queryType.Namespace;
                    var queryElement = xDocument.Descendants(ns + queryType.Name).FirstOrDefault();
                    if (queryElement != null)
                    {
                        var query = Convert.ChangeType(this.Deserialize(queryElement.ToString(), queryType), queryType);
                        queryResponses.Add((IQueryResponse)query);
                        this.log.Debug($"Successfully deserialized snapshot for {query.GetType().Name}.");
                    }
                    else
                    {
                        this.log.Error($"Unable to find snapshot for {queryType}. It's default model data will be used.");
                    }
                }
            }
            catch (Exception e)
            {
                this.log.Error("While loading MKService's snapshot, caught the following exception:", e);
            }

            if (queryResponses.Any())
            {
                this.log.Debug($"Bootstrapper has deserialized {queryResponses.Count} queries for the ServiceTypeProvider to use.");
            }
        }

        /// <summary>
        /// Deserializes the specified snapshot.
        /// </summary>
        /// <param name="snapshot">The snapshot.</param>`
        /// <param name="type">The type.</param>
        /// <returns>The deserialized query.</returns>
        private object Deserialize(string snapshot, Type type)
        {
            var stream = new MemoryStream();
            var data = System.Text.Encoding.UTF8.GetBytes(snapshot);
            stream.Write(data, 0, data.Length);
            stream.Position = 0;
            return new DataContractSerializer(
                type,
                new DataContractSerializerSettings
                {
                    DataContractResolver = new LwiDataContractResolver(this.KnownTypeAssemblies)
                }).ReadObject(stream);
        }

        /// <summary>
        /// Creates a XML writer.
        /// </summary>
        /// <typeparam name="T">Template typed to accept any Stream derieved implementations.</typeparam>
        /// <param name="stream">The stream object reference to which you want to write.</param>
        /// <param name="prettyPrint">
        /// If set to <c>true</c>, the generated output of the writer will be more human readable with inserted newlines
        /// and indentions. Else, the generated output of the writer will be all on one line saving space keeping the
        /// file size lower.
        /// </param>
        /// <returns>An xml writer.</returns>
        private XmlDictionaryWriter GetXmlWriter<T>(ref T stream, bool prettyPrint = false) where T : Stream
        {
            return prettyPrint ?
                XmlDictionaryWriter.CreateDictionaryWriter(new XmlTextWriter(stream, System.Text.Encoding.UTF8) { Formatting = Formatting.Indented, Indentation = 4, IndentChar = ' ' })
                : XmlDictionaryWriter.CreateTextWriter(stream, System.Text.Encoding.UTF8);
        }
    }
}
