

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;

namespace ServerHost
{
    /// <summary>
    /// A parser of command line arguments.
    /// </summary>
    public class CommandLineParser
    {
        /// <summary>
        /// The command line option for the configuration file.
        /// </summary>
        private const string ConfigFileOption = "--config";

        /// <summary>
        /// The command line option for the first help option.
        /// </summary>
        private const string HelpOption1 = "--help";

        /// <summary>
        /// The command line option for the second help option.
        /// </summary>
        private const string HelpOption2 = "/?";

        /// <summary>
        /// The log.
        /// </summary>
        private readonly ILog log = LogManager.GetLogger(typeof(CommandLineParser));

        /// <summary>
        /// Gets the name of the configuration file to use for the application.
        /// </summary>
        /// <value>
        /// If the command line argument was specified, the name of the configuration file to use for the application; otherwise,
        /// <c>null</c>.
        /// </value>
        public string ConfigFile { get; private set; }

        /// <summary>
        /// Gets the error detected by the parser. Only applies if <see cref="HasError" /> is <c>true</c>.
        /// </summary>
        /// <value>
        /// If the parser detected an error while parsing the command line arguments, the error; otherwise, <c>null</c>.
        /// </value>
        public string Error { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the parser detected an error while parsing command line arguments.
        /// </summary>
        /// <value>
        /// <c>true</c>, the parser detected an error; otherwise, <c>false</c>.
        /// </value>
        public bool HasError { get; private set; }

        /// <summary>
        /// Gets the command line argument help screen. Only available if <see cref="ShowHelp" /> is <c>true</c>.
        /// </summary>
        /// <value>
        /// If help was requested on the command line, the help screen; otherwise, <c>null</c>.
        /// </value>
        public string Help { get; private set; }

        /// <summary>
        /// Gets a value indicating whether help was requested on the command line.
        /// </summary>
        /// <value>
        /// <c>true</c>, if help was requested on the command line; otherwise, <c>false</c>.
        /// </value>
        public bool ShowHelp { get; private set; }

        /// <summary>
        /// Parses the specified command line arguments.
        /// </summary>
        /// <param name="args">The command line arguments.</param>
        public void Parse(string[] args)
        {
            if (args == null ||
                args.Length == 0)
            {
                return;
            }

            this.log.DebugFormat("Parsing command line arguments: {0}.", string.Join(",", args));

            if (args.Any(x => string.Equals(x, HelpOption1, StringComparison.OrdinalIgnoreCase)) ||
                args.Any(x => string.Equals(x, HelpOption2, StringComparison.OrdinalIgnoreCase)))
            {
                this.log.Debug("Detected help command line argument.");

                this.BuildHelp();

                this.ShowHelp = true;

                return;
            }

            var configFileArgument = args.LastOrDefault(
                x => x.StartsWith(ConfigFileOption, StringComparison.OrdinalIgnoreCase));

            if (configFileArgument != null)
            {
                this.log.Debug("Detected config file command line argument.");

                this.ConfigFile = this.ParseQuotedStringOption(ConfigFileOption, configFileArgument);

                if (this.HasError)
                {
                    this.log.ErrorFormat(
                        "Parsing config file command line argument resulted in error: {0}.",
                        this.Error);

                    return;
                }
            }

            this.log.Debug("Successfully parsed all command line arguments.");
        }

        /// <summary>
        /// Builds the command line arguments help screen.
        /// </summary>
        private void BuildHelp()
        {
            var builder = new StringBuilder();

            builder.AppendLine("ServerHost.exe");
            builder.AppendLine("LWI service host application.");
            builder.AppendLine();

            var options = new Dictionary<string, string>
            {
                {
                    ConfigFileOption,
                    "If present, the name of the application configuration file to use. Should be surrounded in double-quotes."
                },
                {
                    HelpOption1,
                    "Show this screen."
                },
                {
                    HelpOption2,
                    "Show this screen."
                }
            };

            foreach (var option in options.OrderBy(x => x.Key))
            {
                builder.AppendLine(option.Key);

                var description = option.Value;

                while (description.Length > 0)
                {
                    var line = description.Substring(0, Math.Min(76, description.Length));

                    var lineBreak = line.Length < description.Length;

                    if (lineBreak)
                    {
                        var length = line.LastIndexOf(" ", StringComparison.Ordinal);

                        if (length > 0)
                        {
                            line = line.Remove(length);
                        }
                    }

                    description = description.Substring(Math.Min(line.Length + 1, description.Length));

                    builder.Append(new string(' ', 4));
                    builder.AppendLine(line);
                }
            }

            this.Help = builder.ToString();
        }

        /// <summary>
        /// Parses an quoted string command line argument option.
        /// </summary>
        /// <param name="option">The command line option name.</param>
        /// <param name="arg">The actual command line argument.</param>
        /// <returns>
        /// If an error occurred, <c>null</c>; otherwise, the parsed quoted string command line argument option.
        /// </returns>
        private string ParseQuotedStringOption(string option, string arg)
        {
            var pieces = arg.Split(new[] { '=' }, StringSplitOptions.RemoveEmptyEntries);

            if (pieces.Length == 1)
            {
                this.Error =
                    $"Command line argument name, '{option}', was expected to be followed by '=' and a value enclosed in double-quotes.";
                this.HasError = true;

                return null;
            }

            return pieces[1].TrimStart('"', '\'').TrimEnd('"', '\'');
        }
    }
}