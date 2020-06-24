

using System.Linq;
using System.Text;

namespace Service
{
    /// <summary>
    /// Provides extension methods for working with <see cref="IMessage" />
    /// </summary>
    public static class MessageExtensions
    {
        /// <summary>
        /// Dumps the message and the values of it's public properties to a string, if possible.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns>
        /// The name and contents of the message as a string. If an error occurs, just the name of the message.
        /// </returns>
        public static string Dump(this IMessage message)
        {
            if (message == null)
            {
                return "(null)";
            }

            try
            {
                var builder = new StringBuilder();

                builder.AppendLine($"{message.GetType().Name}:");
                builder.AppendLine("{");

                var properties = message.GetType().GetProperties().OrderBy(x => x.Name);

                foreach (var property in properties)
                {
                    var value = property.GetValue(message);
                    var valueString = value == null ? "(null)" : value.ToString();

                    builder.AppendLine($"\t{property.Name}: {valueString}");
                }

                builder.Append("}");

                return builder.ToString();
            }
            catch
            {
                return message.GetType().Name;
            }
        }
    }
}