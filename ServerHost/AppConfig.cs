using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ServerHost
{
    public abstract class AppConfig : IDisposable
    {
        /// <summary>
        /// Changes the current application configuration file to the configuration file at the specified path.
        /// </summary>
        /// <param name="path">The new application configuration file path.</param>
        /// <returns>
        /// An instance of <see cref="AppConfig" />.
        /// </returns>
        public static AppConfig Change(string path)
        {
            return new ActiveAppConfig(path);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged
        /// resources.
        /// </summary>
        public abstract void Dispose();

        /// <summary>
        /// A private class that handles swapping the current application configuration file.
        /// </summary>
        private sealed class ActiveAppConfig : AppConfig
        {
            /// <summary>
            /// The original application configuration file.
            /// </summary>
            private readonly string originalConfigPath = AppDomain.CurrentDomain.GetData("APP_CONFIG_FILE").ToString();

            /// <summary>
            /// <c>true</c>, if the object has been disposed; otherwise, <c>false</c>.
            /// </summary>
            private bool isDisposed;

            /// <summary>
            /// Initializes a new instance of the <see cref="AppConfig.ActiveAppConfig" /> class.
            /// </summary>
            /// <param name="path">The new application configuration file path.</param>
            public ActiveAppConfig(string path)
            {
                AppDomain.CurrentDomain.SetData("APP_CONFIG_FILE", path);

                ResetConfiguration();
            }

            /// <summary>
            /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged
            /// resources.
            /// </summary>
            public override void Dispose()
            {
                if (!this.isDisposed)
                {
                    AppDomain.CurrentDomain.SetData("APP_CONFIG_FILE", this.originalConfigPath);

                    ResetConfiguration();

                    this.isDisposed = true;
                }

                // ReSharper disable once GCSuppressFinalizeForTypeWithoutDestructor
                GC.SuppressFinalize(this);
            }

            /// <summary>
            /// Resets the configuration settings for the current application.
            /// </summary>
            [SuppressMessage(
                "ReSharper",
                "PossibleNullReferenceException",
                Justification = "False positive null reference warnings.")]
            private static void ResetConfiguration()
            {
                // ReSharper disable once InconsistentNaming
                const BindingFlags Flags = BindingFlags.NonPublic | BindingFlags.Static;

                typeof(ConfigurationManager)
                    .GetField("s_initState", Flags)
                    .SetValue(null, 0);

                typeof(ConfigurationManager)
                    .GetField("s_configSystem", Flags)
                    .SetValue(null, null);

                typeof(ConfigurationManager)
                    .Assembly
                    .GetTypes()
                    .First(x => x.FullName == "System.Configuration.ClientConfigPaths")
                    .GetField("s_current", Flags)
                    .SetValue(null, null);
            }
        }
    }
}
