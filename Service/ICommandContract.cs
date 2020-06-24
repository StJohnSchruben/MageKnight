

using System.ServiceModel;

namespace Service
{
    /// <summary>
    /// Represents the service contract for command messages.
    /// </summary>
    [ServiceContract(CallbackContract = typeof(ICommandCallbackContract), Namespace = ServiceConstants.QueryNamespace)]
    public interface ICommandContract
    {
        /// <summary>
        /// Occurs when a new command message is published.
        /// </summary>
        event PublishedEventHandler MessagePublished;

        /// <summary>
        /// Occurs when a new client subscribes to new command messages.
        /// </summary>
        event SubscriptionEventHandler MessageSubscribed;

        /// <summary>
        /// Publishes the specified command message.
        /// </summary>
        /// <param name="message">The command message.</param>
        [OperationContract(IsOneWay = true)]
        void Publish(IMessage message);

        /// <summary>
        /// Subscribes to the specified command message type.
        /// </summary>
        /// <param name="messageType">The command message type.</param>
        [OperationContract(IsOneWay = true)]
        void Subscribe(string messageType);
    }
}