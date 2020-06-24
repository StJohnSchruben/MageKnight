

using System.ServiceModel;

namespace Service
{
    /// <summary>
    /// Represents the service contract for command message callbacks.
    /// </summary>
    [ServiceContract(Namespace = ServiceConstants.CommandNamespace)]
    public interface ICommandCallbackContract
    {
        /// <summary>
        /// Handles the subscription of the specified command message.
        /// </summary>
        /// <param name="message">The command message.</param>
        [OperationContract(IsOneWay = true)]
        void HandleSubscribe(IMessage message);
    }
}