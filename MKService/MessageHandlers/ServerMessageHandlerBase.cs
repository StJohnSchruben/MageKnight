using MKService.ModelUpdaters;
using MKService.Updates;
using Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MKService.MessageHandlers
{
    internal abstract class ServerMessageHandlerBase<TMessage, TQuery, TQueryComponent> : IServerMessageHandler
        where TMessage : class, IMessage where TQuery : class, IQueryResponse where TQueryComponent : class, IUpdatable
    {
        /// <summary>
        /// The model updater resolver.
        /// </summary>
        private readonly IModelUpdaterResolver modelUpdaterResolver;

        /// <summary>
        /// Initializes a new instance of the <see cref="ServerMessageHandlerBase{TMessage,TQuery,TQueryComponent}" /> class.
        /// </summary>
        /// <param name="modelUpdaterResolver">The model updater resolver.</param>
        protected ServerMessageHandlerBase(IModelUpdaterResolver modelUpdaterResolver)
        {
            this.modelUpdaterResolver = modelUpdaterResolver;
        }

        /// <summary>
        /// Determine if the handler can handle the specified message given the specified query.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="query">The query.</param>
        /// <returns><c>true</c>, if the handler can handle the message; otherwise, <c>false</c>.</returns>
        public bool CanHandle(IMessage message, IQueryResponse query)
        {
            if (message == null || query == null)
            {
                return false;
            }

            return typeof(TMessage) == message.GetType() && typeof(TQuery) == query.GetType();
        }

        /// <summary>
        /// Determines whether this instance can handle the specified message type.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns>
        /// <c>true</c>, if this instance can handle the specified message type; otherwise, <c>false</c>.
        /// </returns>
        public bool CanHandleMessageType(IMessage message)
        {
            if (message == null)
            {
                return false;
            }

            return typeof(TMessage) == message.GetType();
        }

        /// <summary>
        /// Handles the specified message by updating the specified query to reflect the state changes described in the
        /// specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="query">The query to be updated.</param>
        public void Handle(IMessage message, IQueryResponse query)
        {
            var msg = message as TMessage;
            var q = query as TQuery;

            if (msg == null || q == null)
            {
                return;
            }

            var component = this.LocateQueryComponent(msg, q);

            if (component == null)
            {
                return;
            }

            this.modelUpdaterResolver.GetUpdater<TQueryComponent, TMessage>().Update(component, msg);
        }

        /// <summary>
        /// Locates the query or query component of the specified query that needs to be updated in response to the
        /// specified message. This method should not actually update the query as this is handled in the model updater.
        /// It is safe to return <c>null</c> from this method.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="query">The query to be updated.</param>
        /// <returns>
        /// If the query or query component affected by the specified message is found, the query or query component;
        /// otherwise, <c>null</c>.
        /// </returns>
        protected abstract TQueryComponent LocateQueryComponent(TMessage message, TQuery query);
    }
}
