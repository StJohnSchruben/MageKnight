using GalaSoft.MvvmLight.Command;
using log4net;
using log4net.Appender;
using log4net.Core;
using log4net.Layout;
using log4net.Repository.Hierarchy;
using MKViewModel;
using ReDefNet;
using ServerHost;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace ProjectLauncher
{
    public enum MyServiceMode
    {
        NoService = 0,
        Local = 1,
        Remote = 2
    }

    public class StartUpController : ObservableObject
    {

        /// <summary>
        /// The background threads manual reset event.
        /// </summary>
        private readonly ManualResetEvent backgroundThreadsManualResetEvent = new ManualResetEvent(false);

        /// <summary>
        /// The green led color.
        /// </summary>
        private readonly Brush greenLedColor = new SolidColorBrush(Colors.Lime);

        /// <summary>
        /// The red led color.
        /// </summary>
        private readonly Brush redLedColor = new SolidColorBrush(Colors.Red);

        /// <summary>
        /// The hosting service type.
        /// </summary>
        private MyServiceMode hostingServiceType;

        /// <summary>
        /// The is GameService button enabled.
        /// </summary>
        private bool isGameServiceButtonEnabled;

        /// <summary>
        /// The is GameService with workstation button enabled.
        /// </summary>
        private bool isGameServiceWorkstationButtonEnabled;

        /// <summary>
        /// The log.
        /// </summary>
        private ILog log;

        /// <summary>
        /// The GameService domain.
        /// </summary>
        private MageKnight2D.MainWindow GameServiceDomain;

        /// <summary>
        /// The GameService domain with workstation.
        /// </summary>
        private Process workstationProcess;

        /// <summary>
        /// The server.
        /// </summary>
        private IServer server;

        /// <summary>
        /// The services thread.
        /// </summary>
        private Thread servicesThread;

        /// <summary>
        /// Initializes a new instance of the <see cref="StartUpController"/> class.
        /// </summary>
        public StartUpController()
        {
            this.ServiceSelector = new ServiceTypeSelector();
            this.HostingServiceType = MyServiceMode.Local;

            var executingDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var logFilePath = executingDirectory + "\\Logs\\GameService.log";

            var hierarchy = (Hierarchy)LogManager.GetRepository();

            hierarchy.Root.RemoveAllAppenders();

            var patternLayout = new PatternLayout
            {
                ConversionPattern = @"%date [%thread] %-5level %logger - %message%newline%exception"
            };
            patternLayout.ActivateOptions();

            var appender = new RollingFileAppender
            {
                AppendToFile = false,
                CountDirection = 1,
                Encoding = Encoding.UTF8,
                File = logFilePath,
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

            this.log = LogManager.GetLogger(typeof(StartUpController));
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is hosting service.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is hosting service; otherwise, <c>false</c>.
        /// </value>
        public MyServiceMode HostingServiceType
        {
            get
            {
                return this.hostingServiceType;
            }

            set
            {
                var changed = this.Set(() => this.HostingServiceType, ref this.hostingServiceType, value);

                if (!changed)
                {
                    return;
                }

                switch (value)
                {
                    case MyServiceMode.NoService:
                        {
                            this.UpdateButtons(true);
                            this.RaisePropertyChanged(() => this.IsServiceTypeSelectable);
                            break;
                        }

                    case MyServiceMode.Local:
                        {
                            this.UpdateButtons(false);
                            this.RaisePropertyChanged(() => this.IsServiceTypeSelectable);
                            break;
                        }

                    case MyServiceMode.Remote:
                        {
                            this.UpdateButtons(false);
                            this.RaisePropertyChanged(() => this.IsServiceTypeSelectable);
                            break;
                        }
                }
            }
        }

        /// <summary>
        /// Gets the initialize led.
        /// </summary>
        /// <value>
        /// The initialize led.
        /// </value>
        public Brush InitializeLed
        {
            get
            {
                return this.server?.IsOpen ?? false
                    ? this.greenLedColor
                    : this.redLedColor;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance start button has been enabled.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is start TXPA button enabled; otherwise, <c>false</c>.
        /// </value>
        public bool IsGameServiceButtonEnabled
        {
            get { return this.isGameServiceButtonEnabled; }
            set { this.Set(() => this.IsGameServiceButtonEnabled, ref this.isGameServiceButtonEnabled, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance start button has been enabled.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is start TXPA button enabled; otherwise, <c>false</c>.
        /// </value>
        public bool IsGameServiceWorkstationButtonEnabled
        {
            get { return this.isGameServiceWorkstationButtonEnabled; }
            set { this.Set(() => this.IsGameServiceWorkstationButtonEnabled, ref this.isGameServiceWorkstationButtonEnabled, value); }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is service type selectable.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is service type selectable; otherwise, <c>false</c>.
        /// </value>
        public bool IsServiceTypeSelectable => !(this.HostingServiceType == MyServiceMode.NoService);

        /// <summary>
        /// Gets the on start up closing.
        /// </summary>
        /// <value>
        /// The on start up closing.
        /// </value>
        public ICommand OnStartUpClosing => new RelayCommand(this.CloseStartup);

        /// <summary>
        /// Gets the service selector.
        /// </summary>
        /// <value>
        /// The service selector.
        /// </value>
        public IServiceTypeSelector ServiceSelector { get; }

        /// <summary>
        /// Gets the GameService command to open the domain.
        /// </summary>
        /// <value>
        /// The start of GameService domain.
        /// </value>
        public ICommand StartGameServiceDomainCommand => new RelayCommand(this.StartGameServiceDomain);

        /// <summary>
        /// Gets the initialize service command.
        /// </summary>
        /// <value>
        /// The initialize service command.
        /// </value>
        public ICommand StartServiceButtonCommand => new RelayCommand(this.ServiceDeterminate);

        /// <summary>
        /// Gets a value indicating whether this instance is server open.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is server open; otherwise, <c>false</c>.
        /// </value>
        public bool StartServiceButtonEnabled => !this.server?.IsOpen ?? true;

        /// <summary>
        /// Gets the stop service button command.
        /// </summary>
        /// <value>
        /// The stop service button command.
        /// </value>
        public ICommand StopServiceButtonCommand => new RelayCommand(() =>
        {
            this.backgroundThreadsManualResetEvent.Set();
            this.backgroundThreadsManualResetEvent.Reset();
        });

        /// <summary>
        /// Gets a value indicating whether this instance is server open.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is server open; otherwise, <c>false</c>.
        /// </value>
        public bool StopServiceButtonEnabled => (this.server?.IsOpen ?? false);

        /// <summary>
        /// Extracts the exception.
        /// </summary>
        /// <param name="e">The exception.</param>
        /// <returns>
        /// Returns the string representation of the exception.
        /// </returns>
        internal string ExtractException(Exception e)
        {
            var builder = new StringBuilder();
            builder.AppendLine($"Exception type: {e.GetType()}");
            builder.AppendLine($"Message: {e.Message}");

            if (e.StackTrace != null)
            {
                builder.AppendLine();
                var trace = e.StackTrace.Split(Environment.NewLine.ToCharArray());
                builder.AppendLine($"The stack trace for the exception is as follows: ");
                foreach (var item in trace)
                {
                    if (item == string.Empty)
                    {
                        continue;
                    }

                    builder.AppendLine($" * {item}");
                }
            }

            if (e.InnerException != null)
            {
                builder.AppendLine();
                builder.AppendLine("Inner Exception:");
                builder.Append(this.ExtractException(e.InnerException));
            }

            return builder.ToString();
        }

        /// <summary>
        /// Safely executes.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <param name="exceptionHandler">The exception handler.</param>
        private static void SafeExecute(Action action, Action<Exception> exceptionHandler)
        {
            try
            {
                action.Invoke();
            }
            catch (Exception e)
            {
                exceptionHandler.Invoke(e);
            }
        }

        /// <summary>
        /// Closes the startup.
        /// </summary>
        private void CloseStartup()
        {
            this.backgroundThreadsManualResetEvent.Set();
            this.server?.Close();

            this.GameServiceDomain?.Close();
            this.workstationProcess?.Close();
        }

        /// <summary>
        /// Initializes the log.
        /// </summary>
        private void InitializeLog()
        {
            var hierarchy = (Hierarchy)LogManager.GetRepository();

            hierarchy.Root.RemoveAllAppenders();

            var patternLayout = new PatternLayout
            {
                ConversionPattern = @"%date [%thread] %-5level %logger - %message%newline%exception"
            };
            patternLayout.ActivateOptions();

            var appender = new RollingFileAppender
            {
                AppendToFile = false,
                CountDirection = 1,
                Encoding = System.Text.Encoding.UTF8,
                File = @"Logs\GameServiceLogs.log",
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

            this.log = LogManager.GetLogger(typeof(App));
        }

        /// <summary>
        /// Initializes the service.
        /// </summary>
        private void InitializeService()
        {
            this.server.Open();

            this.backgroundThreadsManualResetEvent.WaitOne();
            this.server.Close();
            this.server.Dispose();
            this.server = null;
            this.StartUpControllerUpdate(false);
        }

        /// <summary>
        /// Services the determinate.
        /// </summary>
        private void ServiceDeterminate()
        {
            MK2DStartupSettings.UseService = true;

            IPEndPoint endpoint = null;
            if (this.HostingServiceType == MyServiceMode.Local)
            {
                endpoint = new IPEndPoint(this.ServiceSelector.LocalIP, 9000);
                this.server = new Server();
                this.server.Opened += (s, e) => this.StartUpControllerUpdate();

                this.StartNewServices();
            }
            else if (this.HostingServiceType == MyServiceMode.Remote)
            {
                var array = new byte[]
                {
                    this.ServiceSelector.FirstOctet,
                    this.ServiceSelector.SecondOctet,
                    this.ServiceSelector.ThirdOctet,
                    this.ServiceSelector.ForthOctet
                };

                endpoint = new IPEndPoint(new IPAddress(array), 9000);
                this.StartUpControllerUpdate();
            }

            MK2DStartupSettings.WstServiceEndPoint = endpoint;
        }

        /// <summary>
        /// Starts the n c3 domain.
        /// </summary>
        private void StartGameServiceDomain()
        {
            this.GameServiceDomain = new MageKnight2D.MainWindow(); // This is the GameService Standalone application
            this.GameServiceDomain.Closed += (sender, e) =>
            {
                this.IsGameServiceButtonEnabled = true;
            };
            this.IsGameServiceButtonEnabled = false;
            this.GameServiceDomain.Show();
        }

   
        /// <summary>
        /// Starts the new services.
        /// </summary>
        private void StartNewServices()
        {
            this.servicesThread = new Thread(() => SafeExecute(
                this.InitializeService,
                (e) => MessageBox.Show(e.Message)))
            {
                IsBackground = true,
                Name = "MageKnight New Service Thread"
            };

            this.servicesThread.Start();
        }

        /// <summary>
        /// Starts up controller initialized.
        /// </summary>
        /// <param name="activateButtons">If set to true activate buttons.</param>
        private void StartUpControllerUpdate(bool activateButtons = true)
        {
            this.UpdateButtons(activateButtons);
            this.RaisePropertyChanged(() => this.InitializeLed);
            this.RaisePropertyChanged(() => this.StartServiceButtonEnabled);
            this.RaisePropertyChanged(() => this.StopServiceButtonEnabled);
        }

        /// <summary>
        /// Enables or disables the buttons.
        /// </summary>
        /// <param name="value">The value to set the client application buttons enabled status.</param>
        private void UpdateButtons(bool value)
        {
            this.IsGameServiceButtonEnabled = value;
            this.IsGameServiceWorkstationButtonEnabled = value;
        }
    }
}
