using Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MKService.MessageHandlers
{
    internal class ServerMessageHandlerResolver : IServerMessageHandlerResolver
    {
        /// <summary>
        /// The handlers.
        /// </summary>
        private readonly IEnumerable<IServerMessageHandler> handlers;

        /// <summary>
        /// Initializes a new instance of the <see cref="ServerMessageHandlerResolver" /> class.
        /// </summary>
        /// <param name="handlers">The server message handlers.</param>
        public ServerMessageHandlerResolver(IEnumerable<IServerMessageHandler> handlers)
        {

            this.handlers = handlers;
        }

        /// <summary>
        /// Gets the server message handlers that can handle the specified message and query. This method will
        /// intentionally throw an exception if no handlers could be found.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="query">The query.</param>
        /// <returns>A collection of message handlers.</returns>
        /// <exception cref="System.ArgumentException">Argument Exception.</exception>
        public IEnumerable<IServerMessageHandler> GetHandlers(IMessage message, IQueryResponse query)
        {
            if (!this.handlers.Any(h => h.CanHandleMessageType(message)))
            {
                throw new ArgumentException(
                    $"Could not locate any server message handlers for the message type, '{message.GetType()}', " +
                    $"and the query type, '{query.GetType()}'. Did you forget to write a server message handler?");
            }

            var result = this.handlers.Where(x => x.CanHandle(message, query)).ToList();

            return result;
        }
    }
}
