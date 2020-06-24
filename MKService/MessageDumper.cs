using Service;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MKService
{
    internal static class MessageDumper
    {
        /// <summary>
        /// The logger.
        /// </summary>
        private static log4net.ILog log = log4net.LogManager.GetLogger(typeof(MessageDumper));

        /// <summary>
        /// Dumps the specified message to the debug output. Only available in debug builds.
        /// </summary>
        /// <typeparam name="TMessage">The message type.</typeparam>
        /// <param name="message">The message.</param>
        /// <param name="header">The header message to include before the message dump.</param>
        [Conditional("DEBUG")]
        public static void DumpMessage<TMessage>(TMessage message, string header) where TMessage : IMessage
        {
            if (message == null)
            {
                return;
            }

            try
            {
                Debug.WriteLine($"{header}:\r\n{message.Dump()}");
            }
            catch
            {
                // Fail silently, since this is just a helper. It shouldn't crash the application.
            }
        }
    }
}
