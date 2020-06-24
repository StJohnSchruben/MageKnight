using log4net;
using MKService.QueryFactories;
using Service;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MKService
{
    internal sealed class NullServiceClient : IServiceClient
    {
        /// <summary>
        /// The log.
        /// </summary>
        private readonly ILog log = LogManager.GetLogger(typeof(NullServiceClient));

        /// <summary>
        /// The message origination hash.
        /// </summary>
        private readonly int originationHash;

        /// <summary>
        /// The query factory resolver.
        /// </summary>
        private readonly IQueryFactoryResolver queryFactoryResolver;

        /// <summary>
        /// Initializes a new instance of the <see cref="NullServiceClient" /> class.
        /// </summary>
        /// <param name="queryFactoryResolver">The query factory resolver.</param>
        public NullServiceClient(IQueryFactoryResolver queryFactoryResolver)
        {
            this.originationHash = Process.GetCurrentProcess().Id;
            this.queryFactoryResolver = queryFactoryResolver;
        }

        /// <summary>
        /// Occurs when a new message is received.
        /// </summary>
#pragma warning disable CS0067
        public event EventHandler<PublishedEventArgs> MessageReceived;

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
        }

        /// <summary>
        /// Asynchronously publishes the specified message.
        /// </summary>
        /// <typeparam name="TMessage">The message type.</typeparam>
        /// <param name="message">The message.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task PublishAsync<TMessage>(TMessage message) where TMessage : IMessage
        {
            this.log.DebugFormat(
                "Simulating publishing message of type '{0}' (not connected to service) with contents:\r\n{1}.",
                typeof(TMessage).FullName,
                message.Dump());

            MessageDumper.DumpMessage(message, "Simulating publishing message (not connected to service)");

            await Task.FromResult(0);
        }

        /// <summary>
        /// Executes the specified query.
        /// </summary>
        /// <typeparam name="TQuery">The query type.</typeparam>
        /// <returns>The query result.</returns>
        public TQuery Query<TQuery>() where TQuery : class, IQueryResponse
        {
            this.log.DebugFormat(
                "Simulating querying for query of type '{0}' (not connected to service).",
                typeof(TQuery).FullName);

            return this.queryFactoryResolver.GetFactory<TQuery>().Create();
        }

        /// <summary>
        /// Asynchronously subscribes to the specified message type.
        /// </summary>
        /// <typeparam name="TMessage">The message type.</typeparam>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task SubscribeAsync<TMessage>() where TMessage : IMessage
        {
            this.log.DebugFormat(
                "Simulating subscribing to messages of type '{0}' (not connected to service).",
                typeof(TMessage).FullName);

            await Task.FromResult(0);
        }
    }
}
