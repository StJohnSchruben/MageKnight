using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Service;

namespace MKService.MessageHandlers
{
    internal interface IServerMessageHandlerResolver
    {
        /// <summary>
        /// Gets the handlers.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="query">The query.</param>
        /// <returns>The Handlers.</returns>
        IEnumerable<IServerMessageHandler> GetHandlers(IMessage message, IQueryResponse query);
    }
}
