
using System;
using System.Collections.Generic;
using System.ServiceModel;
using log4net;

using Service;
using MKService.MessageHandlers;

namespace MKService.QueryHandlers
{
    /// <summary>
    /// The query handler base.
    /// </summary>
    /// <typeparam name="TQuery">The type of the query.</typeparam>
    /// <seealso cref="Service.IQueryHandler" />
    internal abstract class QueryHandlerBase<TQuery> : IQueryHandler where TQuery : IQueryResponse
    {
        /// <summary>
        /// The log.
        /// </summary>
        private readonly ILog log = LogManager.GetLogger(typeof(QueryHandlerBase<TQuery>));

        /// <summary>
        /// The message handler resolver.
        /// </summary>
        private readonly IServerMessageHandlerResolver messageHandlerResolver;

        /// <summary>
        /// The message subscribers.
        /// </summary>
        private readonly Dictionary<string, List<ICommandCallbackContract>> messageSubscribers =
           new Dictionary<string, List<ICommandCallbackContract>>();

        /// <summary>
        /// The thread synchronizing object.
        /// </summary>
        private readonly object padlock = new object();

        /// <summary>
        /// The query response.
        /// </summary>
        private readonly TQuery response;

        /// <summary>
        /// The supported query type.
        /// </summary>
        private readonly string supportedQueryType = typeof(TQuery).ToString();

        /// <summary>
        /// The next query handler in the chain.
        /// </summary>
        private IQueryHandler nextQueryHandler;

        /// <summary>
        /// Initializes a new instance of the <see cref="QueryHandlerBase{TQuery}" /> class.
        /// </summary>
        /// <param name="queryContract">The query contract.</param>
        /// <param name="commandContract">The command contract.</param>
        /// <param name="nextQueryHandler">The next query handler in the chain.</param>
        /// <param name="defaultQueryResponse">The default query response.</param>
        /// <param name="messageHandlerResolver">The message handler resolver.</param>
        protected QueryHandlerBase(
            IQueryContract queryContract,
            ICommandContract commandContract,
            IQueryHandler nextQueryHandler,
            TQuery defaultQueryResponse,
            IServerMessageHandlerResolver messageHandlerResolver)
        {
            this.response = defaultQueryResponse;
            this.messageHandlerResolver = messageHandlerResolver;

            nextQueryHandler.AddHandler(this);

            queryContract.QuerySubmitted += this.OnQuerySubmitted;

            commandContract.MessagePublished += this.OnMessagePublished;
            commandContract.MessageSubscribed += this.OnMessageSubscribed;
        }

        /// <summary>
        /// Adds the specified query handler.
        /// </summary>
        /// <param name="handler">The query handler.</param>
        /// <remarks>
        /// If the internal next handler is <c>null</c>, this method should set the next internal handler to the
        /// specified handler; otherwise, it should pass the specified handler to the next internal handler's <see
        /// cref="M:Service.IQueryHandler.AddHandler(Service.IQueryHandler)" /> method.
        /// </remarks>
        public void AddHandler(IQueryHandler handler)
        {
            if (this.nextQueryHandler == null)
            {
                this.nextQueryHandler = handler;
            }
            else
            {
                this.nextQueryHandler.AddHandler(handler);
            }
        }

        /// <summary>
        /// Handles the specified query.
        /// </summary>
        /// <param name="queryType">The query type.</param>
        /// <param name="queryDef">The query definition.</param>
        /// <returns>The query response of the specified query type.</returns>
        /// <remarks>
        /// This method should handle the message. If the internal next handler is <c>null</c>, this method should also
        /// return <c>null</c>; otherwise, it should return the result from the next internal handler's <see
        /// cref="M:Service.IQueryHandler.Handle(System.String,Service.IQueryDef)" /> method.
        /// </remarks>
        public IQueryResponse Handle(string queryType, IQueryDef queryDef)
        {
            var queryResponse = queryType.Equals(this.supportedQueryType)
                ? this.response
                : this.nextQueryHandler?.Handle(queryType, queryDef);

            return queryResponse;
        }

        /// <summary>
        /// Called when a new command message is published.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="PublishedEventArgs" /> instance containing the event data.</param>
        private void OnMessagePublished(object sender, PublishedEventArgs e)
        {
            var messageType = e.Message.GetType();

            IEnumerable<IServerMessageHandler> handlers;

            try
            {
                handlers = this.messageHandlerResolver.GetHandlers(e.Message, this.response);
            }
            catch (ArgumentException ex)
            {
                if (ex.Message != null && ex.Message.Contains("Could not locate any server message handlers"))
                {
                    return;
                }

                throw;
            }
            catch(OutOfMemoryException ome)
            {
                this.log.Error(ome);
                this.log.Error(Environment.StackTrace);
                //throw; testing
                return;
            }
            catch (Exception ex)
            {
                this.log.Error(ex);
                this.log.Error(Environment.StackTrace);
                //throw; testing
                return;
            }

            var isMessageHandled = false;

            foreach (var handler in handlers)
            {
                try
                {
                    handler.Handle(e.Message, this.response);
                    isMessageHandled = true;
                }
                catch (Exception exception)
                {
                    this.log.Error($"{nameof(handler)} has an error in the handle function when receiving a WCF message. ", exception);
                }
            }

            if (!isMessageHandled)
            {
                return;
            }

            try
            {
                var messageTypeString = messageType.ToString();
                if (!this.messageSubscribers.ContainsKey(messageTypeString))
                {
                    return;
                }

                lock (this.padlock)
                {
                    var subscribers = this.messageSubscribers[messageTypeString];
                    var unsubscribers = new List<ICommandCallbackContract>();
                    foreach (var subscriber in subscribers)
                    {
                        try
                        {
                            subscriber.HandleSubscribe(e.Message);
                        }
                        catch (CommunicationObjectAbortedException ee)
                        {
                            this.log.Debug(ee.ToString());
                            unsubscribers.Add(subscriber);
                        }
                    }

                    foreach (var unsubscriber in unsubscribers)
                    {
                        subscribers.Remove(unsubscriber);
                    }
                }
            }
            catch (Exception ex)
            {
                this.log.Error(ex);
            }
        }

        /// <summary>
        /// Called when a new command message type is subscribed to.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="SubscribedEventArgs" /> instance containing the event data.</param>
        private void OnMessageSubscribed(object sender, SubscribedEventArgs e)
        {
            lock (this.padlock)
            {
                if (this.messageSubscribers.ContainsKey(e.MessageType))
                {
                    var callbacks = this.messageSubscribers[e.MessageType];

                    callbacks.Add(e.Callback);
                }
                else
                {
                    var list = new List<ICommandCallbackContract> { e.Callback };
                    this.messageSubscribers.Add(
                        e.MessageType,
                        list);
                }
            }
        }

        /// <summary>
        /// Called when a new query request is submitted.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="QueryArgs" /> instance containing the event data.</param>
        private void OnQuerySubmitted(object sender, QueryArgs e)
        {
            try
            {
                if (!e.QueryType.Equals(this.supportedQueryType))
                {
                    return;
                }

                e.Callback.HandleQueryResponse(this.response);
            }
            catch (Exception ex)
            {
                this.log.Error(ex);
            }
        }
    }
}