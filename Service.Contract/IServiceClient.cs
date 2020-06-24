

using System;
using System.Threading.Tasks;


namespace Service.Contract
{
    /// <summary>
    /// Represents a client of the WST service.
    /// </summary>
    public interface IServiceClient : IDisposable
    {
        /// <summary>
        /// Occurs when a new message is received.
        /// </summary>
        event EventHandler<PublishedEventArgs> MessageReceived;

        /// <summary>
        /// Asynchronously publishes the specified message.
        /// </summary>
        /// <typeparam name="TMessage">The message type.</typeparam>
        /// <param name="message">The message.</param>
        /// <returns>
        /// A task representing the asynchronous operation.
        /// </returns>
        Task PublishAsync<TMessage>(TMessage message) where TMessage : IMessage;

        /// <summary>
        /// Executes the specified query.
        /// </summary>
        /// <typeparam name="TQuery">The query type.</typeparam>
        /// <returns>
        /// The query result.
        /// </returns>
        TQuery Query<TQuery>() where TQuery : class, IQueryResponse;

        /// <summary>
        /// Asynchronously subscribes to the specified message type.
        /// </summary>
        /// <typeparam name="TMessage">The message type.</typeparam>
        /// <returns>
        /// A task representing the asynchronous operation.
        /// </returns>
        Task SubscribeAsync<TMessage>() where TMessage : IMessage;
    }
}
