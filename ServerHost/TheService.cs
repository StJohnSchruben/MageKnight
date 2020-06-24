

using System;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using log4net;
using log4net.Appender;
using log4net.Core;
using log4net.Layout;
using log4net.Repository.Hierarchy;
using ReDefNet;

namespace ServerHost
{
    /// <summary>
    /// The Windows service for the Block II WCF service.
    /// </summary>
    /// <seealso cref="System.ServiceProcess.ServiceBase" />
    public class TheService : ServiceBase
    {
        /// <summary>
        /// The port.
        /// </summary>
        public static string Port;

        /// <summary>
        /// The timeout, in seconds, for the services to startup.
        /// </summary>
        private const int ServiceTimeOutSeconds = 5;

        /// <summary>
        /// The server manual reset event.
        /// </summary>
        private static readonly ManualResetEvent serverManualResetEvent = new ManualResetEvent(false);

        /// <summary>
        /// The logger.
        /// </summary>
        private static ILog log;

        /// <summary>
        /// The application configuration file.
        /// </summary>
        private AppConfig applicationConfig;

        /// <summary>
        /// The server.
        /// </summary>
        private IServer server;

        /// <summary>
        /// Initializes a new instance of the <see cref="TheService" /> class.
        /// </summary>
        public TheService()
        {
            this.AutoLog = true;
            this.CanHandlePowerEvent = false;
            this.CanHandleSessionChangeEvent = false;
            this.CanPauseAndContinue = false;
            this.CanShutdown = true;
            this.CanStop = true;
            this.ServiceName = "WstBlock2Service" + Port;
        }

        /// <summary>
        /// Main entry point for the application.
        /// </summary>
        /// <param name="args">The commandline arguments.</param>
        public static void Main(string[] args)
        {
            string configPath = args[0];
            configPath = configPath.Split('\\').Last();

            for (int i = 0; i < configPath.Length; i++)
            {
                if (Char.IsDigit(configPath[i]))
                {
                    Port += configPath[i];
                }
            }

            InitializeLog();
            var service = new TheService();

            if (Environment.UserInteractive)
            {
                try
                {
                    service.OnStart(args);

                    Console.Title = "Block II Service";
                    Console.WriteLine("Block II service is now running on port: " + Port);
                    Console.WriteLine("Press any key to stop the service.");
                    Console.Read();

                    service.OnStop();

                    Environment.Exit(0);
                }
                catch (Exception e)
                {
                    Console.Error.WriteLine(e);
                    Console.Read();

                    Environment.Exit(1);
                }
            }
            else
            {
                TheService.Run(service);
            }
        }

        /// <summary>
        /// Called when the system is shutting down.
        /// </summary>
        protected override void OnShutdown()
        {
            this.PerformShutdown();
        }

        /// <summary>
        /// Called when a start command is sent to the service by the Service Control Manager (SCM) or when the operating
        /// system starts (for a service that starts automatically).
        /// </summary>
        /// <param name="args">Data passed by the start command.</param>
        protected override void OnStart(string[] args)
        {
            log.InfoFormat(
                "Starting Block II service in {0} mode.",
                Environment.UserInteractive ? "interactive" : "non-interactive");

            if (!Environment.UserInteractive)
            {
                args = Environment.GetCommandLineArgs();
            }

            var parser = new CommandLineParser();
            parser.Parse(args);

            if (parser.HasError)
            {
                log.DebugFormat("Command line parser reported error: {0}.", parser.Error);

                if (Environment.UserInteractive)
                {
                    Console.WriteLine(parser.Error);
                }

                this.Stop();

                return;
            }

            if (parser.ShowHelp)
            {
                log.Debug("Command line help was requested.");

                if (Environment.UserInteractive)
                {
                    Console.WriteLine(parser.Help);
                }
                else
                {
                    log.Error(
                        "Command line help was requested. This is not a valid option when running in non-interactive mode.");
                }

                this.Stop();

                return;
            }

            // ReSharper disable once NotAccessedVariable
            if (parser.ConfigFile != null)
            {
                log.DebugFormat("Using application configuration file '{0}'.", parser.ConfigFile);

                this.applicationConfig = AppConfig.Change(parser.ConfigFile);
            }
            else
            {
                log.Debug("Using default application configuration file.");
            }

            try
            {
                this.server = new Server();
            }
            catch (Exception e)
            {
                log.Error("Creating server threw an exception.", e);

                this.server = null;

                if (Environment.UserInteractive)
                {
                    Console.WriteLine(e);
                }

                this.Stop();

                return;
            }

            this.server.Opened += this.OnServerOpened;
            this.server.Closed += this.OnServerClosed;
            this.server.Faulted += this.OnServerFaulted;

            try
            {
                this.server.Open();
            }
            catch (Exception e)
            {
                log.Error("Opening server threw an exception.", e);

                this.server.Opened -= this.OnServerOpened;
                this.server.Closed -= this.OnServerClosed;
                this.server.Faulted -= this.OnServerFaulted;
                this.server = null;

                if (Environment.UserInteractive)
                {
                    Console.WriteLine(e);
                }

                this.Stop();

                return;
            }

            serverManualResetEvent.WaitOne(TimeSpan.FromSeconds(ServiceTimeOutSeconds));

            if (!this.server.IsOpen)
            {
                log.ErrorFormat("Services failed to start within {0} seconds.", ServiceTimeOutSeconds);

                this.server.Opened -= this.OnServerOpened;
                this.server.Closed -= this.OnServerClosed;
                this.server.Faulted -= this.OnServerFaulted;
                this.server = null;

                if (Environment.UserInteractive)
                {
                    Console.WriteLine("Service failed to start within {0} seconds.", ServiceTimeOutSeconds);
                }

                this.Stop();

                return;
            }

            log.Info("Block II service started successfully.");
        }

        /// <summary>
        /// Called when a stop command is sent to the service by the Service Control Manager (SCM).
        /// </summary>
        protected override void OnStop()
        {
            log.Info("Stopping Block II service.");

            this.PerformShutdown();

            log.Info("Block II service stopped successfully.");
        }

        /// <summary>
        /// Initializes the application logging.
        /// </summary>
        private static void InitializeLog()
        {
            var hierarchy = (Hierarchy)LogManager.GetRepository();

            hierarchy.Root.RemoveAllAppenders();

            GlobalContext.Properties["pid"] = Process.GetCurrentProcess().Id;

            var patternLayout = new PatternLayout
            {
                ConversionPattern = @"%date [%property{pid}] [%thread] %-5level %logger - %message%newline%exception"
            };
            patternLayout.ActivateOptions();

            var appender = new RollingFileAppender
            {
                AppendToFile = false,
                CountDirection = 1,
                Encoding = Encoding.UTF8,
                File = @"Logs\Block2Service" + Port + ".log",
                Layout = patternLayout,
                MaxSizeRollBackups = 20,
                MaximumFileSize = "50MB",
                PreserveLogFileNameExtension = true,
                RollingStyle = RollingFileAppender.RollingMode.Composite,
                StaticLogFileName = true
            };
            appender.ActivateOptions();

            hierarchy.Root.AddAppender(appender);

            hierarchy.Root.Level = Level.Debug;

            hierarchy.Configured = true;

            log = LogManager.GetLogger(typeof(TheService));
        }

        /// <summary>
        /// Called when the server is closed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event data.</param>
        private void OnServerClosed(object sender, EventArgs e)
        {
            if (Environment.UserInteractive)
            {
                Console.WriteLine("The server is now closed.");
            }

            log.Info("The service is now closed.");
        }

        /// <summary>
        /// Called when the server is faulted.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event data.</param>
        private void OnServerFaulted(object sender, EventArgs e)
        {
            if (Environment.UserInteractive)
            {
                Console.WriteLine("The server has faulted. See the log file for more information.");
            }

            log.Error("The service is now faulted.");

            this.Stop();
        }

        /// <summary>
        /// Called when the server is opened.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event data.</param>
        private void OnServerOpened(object sender, EventArgs e)
        {
            if (Environment.UserInteractive)
            {
                Console.WriteLine("The server is now open.");
            }

            serverManualResetEvent.Set();
        }

        /// <summary>
        /// Shuts down the audio router service.
        /// </summary>
        private void PerformShutdown()
        {
            if (this.server == null)
            {
                return;
            }

            this.server.Opened -= this.OnServerOpened;
            this.server.Closed -= this.OnServerClosed;
            this.server.Faulted -= this.OnServerFaulted;

            try
            {
                this.server.Close();
            }
            catch
            {
                // Swallow!
            }
            finally
            {
                this.server = null;
            }

            if (this.applicationConfig == null)
            {
                return;
            }

            try
            {
                this.applicationConfig.Dispose();
            }
            catch
            {
                // Swallow!
            }
            finally
            {
                this.applicationConfig = null;
            }
        }
    }
}