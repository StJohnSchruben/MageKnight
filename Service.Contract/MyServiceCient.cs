

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.ServiceModel;
using System.Threading.Tasks;
using log4net;


namespace Service.Contract
{
    /// <summary>
    /// A client of the DVS service that uses WCF. This class cannot be inherited.
    /// </summary>
    [DebuggerStepThrough]
    public sealed class WstServiceClient : IServiceClient, ICommandCallbackContract, IQueryCallbackContract
    {
        /// <summary>
        /// The logger.
        /// </summary>
        private readonly ILog log = LogManager.GetLogger(typeof(WstServiceClient));

        /// <summary>
        /// The message origination hash.
        /// </summary>
        private readonly int originationHash;

        /// <summary>
        /// The message types that have already been subscribed to.
        /// </summary>
        private readonly HashSet<string> subscribedMessageTypes = new HashSet<string>();

        /// <summary>
        /// The command client.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private ICommandContract commandChannel;

        /// <summary>
        /// The query client.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private IQueryContract queryChannel;

        /// <summary>
        /// Initializes a new instance of the <see cref="WstServiceClient" /> class.
        /// </summary>
        /// <param name="clientChannelProvider">The client channel provider.</param>
        public WstServiceClient(IClientChannelProvider clientChannelProvider)
        {
            if (clientChannelProvider == null)
            {
                throw new ArgumentNullException(nameof(clientChannelProvider));
            }

            this.commandChannel = clientChannelProvider.GetCommandChannel(this);
            this.queryChannel = clientChannelProvider.GetQueryChannel(this);

            this.originationHash = Process.GetCurrentProcess().Id;
        }

        /// <summary>
        /// Occurs when a new message is received.
        /// </summary>
        public event EventHandler<PublishedEventArgs> MessageReceived;

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            if (this.commandChannel != null)
            {
                try
                {
                    // ReSharper disable once SuspiciousTypeConversion.Global
                    ((ICommunicationObject)this.commandChannel).Abort();
                }
                catch
                {
                    // swallow!
                }

                this.commandChannel = null;
            }

            if (this.queryChannel != null)
            {
                try
                {
                    // ReSharper disable once SuspiciousTypeConversion.Global
                    ((ICommunicationObject)this.queryChannel).Abort();
                }
                catch
                {
                    // swallow!
                }

                this.queryChannel = null;
            }
        }

        /// <summary>
        /// Handles the specified query.
        /// </summary>
        /// <param name="query">The query.</param>
        public void HandleQueryResponse(IQueryResponse query)
        {
        }

        /// <summary>
        /// Handles the subscription of the specified command message.
        /// </summary>
        /// <param name="message">The command message.</param>
        public void HandleSubscribe(IMessage message)
        {
            if (message == null)
            {
                return;
            }

            if (message.OriginationHash == this.originationHash)
            {
                this.log.DebugFormat(
                    "Received self-published message of type '{0}'. Message will be ignored.",
                    message.GetType().Name);

                return;
            }

            this.log.DebugFormat(
                "Received published message of type '{0}' and contents:\r\n{1}.",
                message.GetType().Name,
                message.Dump());

            this.MessageReceived?.Invoke(
                this,
                new PublishedEventArgs
                {
                    Message = message
                });
        }

        /// <summary>
        /// Asynchronously publishes the specified message.
        /// </summary>
        /// <typeparam name="TMessage">The message type.</typeparam>
        /// <param name="message">The message.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task PublishAsync<TMessage>(TMessage message) where TMessage : IMessage
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            this.log.DebugFormat(
                "Publishing message of type '{0}' with contents:\r\n{1}.",
                typeof(TMessage).Name,
                message.Dump());

            message.OriginationHash = this.originationHash;

            this.MessageReceived?.Invoke(
                this,
                new PublishedEventArgs
                {
                    Message = message
                });

            await Task.Run(() => this.commandChannel.Publish(message));
        }

        /// <summary>
        /// Executes the specified query.
        /// </summary>
        /// <typeparam name="TQuery">The query type.</typeparam>
        /// <returns>The query result.</returns>
        public TQuery Query<TQuery>() where TQuery : class, IQueryResponse
        {
            var queryType = typeof(TQuery).ToString();

            this.log.DebugFormat("Attempting to retrieve query of type '{0}'.", queryType);

            IQueryResponse result = null;
            try
            {
                result = this.queryChannel.DirectQuery(queryType, null);
            }
            catch (Exception e)
            {
                this.log.Error("An error occurred while trying to query.", e);
            }

            return result as TQuery;
        }

        /// <summary>
        /// Asynchronously subscribes to the specified message type.
        /// </summary>
        /// <typeparam name="TMessage">The message type.</typeparam>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task SubscribeAsync<TMessage>() where TMessage : IMessage
        {
            var messageType = typeof(TMessage).ToString();

            this.log.DebugFormat("Subscribing to messages of type '{0}'.", messageType);

            if (this.subscribedMessageTypes.Contains(messageType))
            {
                return;
            }

            this.subscribedMessageTypes.Add(messageType);

            await Task.Run(() => this.commandChannel.Subscribe(messageType));
        }
    }
}