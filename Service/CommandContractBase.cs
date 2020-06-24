

using System.ServiceModel;

namespace Service
{
    /// <summary>
    /// Base class for command service contracts.
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public abstract class CommandContractBase : ICommandContract
    {
        /// <summary>
        /// Occurs when a new command message is published.
        /// </summary>
        public event PublishedEventHandler MessagePublished;

        /// <summary>
        /// Occurs when a new client subscribes to new command messages.
        /// </summary>
        public event SubscriptionEventHandler MessageSubscribed;

        /// <summary>
        /// Publishes the specified command message.
        /// </summary>
        /// <param name="message">The command message.</param>
        public void Publish(IMessage message)
        {
            var handler = this.MessagePublished;

            handler?.Invoke(
                this,
                new PublishedEventArgs
                {
                    Message = message
                });
        }

        /// <summary>
        /// Subscribes to the specified command message type.
        /// </summary>
        /// <param name="messageType">The command message type.</param>
        public void Subscribe(string messageType)
        {
            var handler = this.MessageSubscribed;

            if (handler == null)
            {
                return;
            }

            var callbackChannel = OperationContext.Current.GetCallbackChannel<ICommandCallbackContract>();

            handler.Invoke(
                this,
                new SubscribedEventArgs
                {
                    MessageType = messageType,
                    Callback = callbackChannel
                });
        }
    }
}