using Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MKService.MessageHandlers
{
    internal interface IServerMessageHandler
    {
        /// <summary>
        /// Determine if the handler can handle the specified message given the specified query.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="query">The query.</param>
        /// <returns>
        /// <c>true</c>, if the handler can handle the message; otherwise, <c>false</c>.
        /// </returns>
        bool CanHandle(IMessage message, IQueryResponse query);

        /// <summary>
        /// Determines whether this instance can handle the specified message type.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns>
        /// <c>true</c>, if this instance can handle the specified message type; otherwise, <c>false</c>.
        /// </returns>
        bool CanHandleMessageType(IMessage message);

        /// <summary>
        /// Handles the specified message by updating the specified query to reflect the state changes described in the specified
        /// message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="query">The query to be updated.</param>
        void Handle(IMessage message, IQueryResponse query);
    }
}
