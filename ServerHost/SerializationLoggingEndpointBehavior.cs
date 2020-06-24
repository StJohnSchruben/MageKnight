

using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

namespace ServerHost
{
    /// <summary>
    /// An endpoint behavior that logs all serialization errors.
    /// </summary>
    public class SerializationLoggingEndpointBehavior : IEndpointBehavior
    {
        /// <summary>
        /// Adds binding parameters during runtime.
        /// </summary>
        /// <param name="endpoint">The endpoint to modify.</param>
        /// <param name="bindingParameters">
        /// The objects that binding elements require to support the behavior.
        /// </param>
        public void AddBindingParameters(ServiceEndpoint endpoint, BindingParameterCollection bindingParameters)
        {
        }

        /// <summary>
        /// Applies the behavior on the client.
        /// </summary>
        /// <param name="endpoint">The endpoint that is to be customized.</param>
        /// <param name="clientRuntime">The client runtime to be customized.</param>
        public void ApplyClientBehavior(ServiceEndpoint endpoint, ClientRuntime clientRuntime)
        {
            ReplaceDefaultSerializerBehavior(endpoint);
        }

        /// <summary>
        /// Applies the behavior on the server.
        /// </summary>
        /// <param name="endpoint">The endpoint that exposes the contract.</param>
        /// <param name="endpointDispatcher">The endpoint dispatcher to be modified or extended.</param>
        public void ApplyDispatchBehavior(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher)
        {
            ReplaceDefaultSerializerBehavior(endpoint);
        }

        /// <summary>
        /// Validates that the endpoint meets some intended criteria.
        /// </summary>
        /// <param name="endpoint">The endpoint to validate.</param>
        public void Validate(ServiceEndpoint endpoint)
        {
        }

        /// <summary>
        /// Replaces the default serialization behavior on each operation on the specified endpoint with the
        /// <see cref="SerializationLoggingOperationBehavior" />.
        /// </summary>
        /// <param name="endpoint">The endpoint to modify.</param>
        private static void ReplaceDefaultSerializerBehavior(ServiceEndpoint endpoint)
        {
            foreach (var operation in endpoint.Contract.Operations)
            {
                var operationBehavior = operation.Behaviors.Find<DataContractSerializerOperationBehavior>();

                if (operationBehavior == null)
                {
                    continue;
                }

                operation.Behaviors.Remove(operationBehavior);

                var loggingBehavior = new SerializationLoggingOperationBehavior(operation)
                {
                    DataContractResolver = operationBehavior.DataContractResolver,
                    DataContractSurrogate = operationBehavior.DataContractSurrogate,
                    IgnoreExtensionDataObject = operationBehavior.IgnoreExtensionDataObject,
                    MaxItemsInObjectGraph = operationBehavior.MaxItemsInObjectGraph
                };

                operation.Behaviors.Add(loggingBehavior);
            }
        }
    }
}