

using System;
using System.Collections.Generic;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

namespace Service
{
    /// <summary>
    /// An attribute used to indicate one or more assemblies that contain types that should be dynamically added as known types
    /// for the service.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface)]
    public class KnownAssemblyAttribute : Attribute, IContractBehavior
    {
        /// <summary>
        /// The data contract resolver.
        /// </summary>
        private readonly LwiDataContractResolver resolver;

        /// <summary>
        /// Initializes a new instance of the <see cref="KnownAssemblyAttribute" /> class.
        /// </summary>
        /// <param name="assemblyNames">The names of the assemblies to scan for known types.</param>
        public KnownAssemblyAttribute(params string[] assemblyNames)
        {
            this.resolver = new LwiDataContractResolver(assemblyNames);
        }

        /// <summary>
        /// Adds the binding parameters.
        /// </summary>
        /// <param name="contractDescription">The contract description.</param>
        /// <param name="endpoint">The endpoint.</param>
        /// <param name="bindingParameters">The binding parameters.</param>
        public void AddBindingParameters(
            ContractDescription contractDescription,
            ServiceEndpoint endpoint,
            BindingParameterCollection bindingParameters)
        {
        }

        /// <summary>
        /// Applies the client behavior.
        /// </summary>
        /// <param name="contractDescription">The contract description.</param>
        /// <param name="endpoint">The endpoint.</param>
        /// <param name="clientRuntime">The client runtime.</param>
        public void ApplyClientBehavior(
            ContractDescription contractDescription,
            ServiceEndpoint endpoint,
            ClientRuntime clientRuntime)
        {
            this.CreateMyDataContractSerializerOperationBehaviors(contractDescription);
        }

        /// <summary>
        /// Applies the dispatch behavior.
        /// </summary>
        /// <param name="contractDescription">The contract description.</param>
        /// <param name="endpoint">The endpoint.</param>
        /// <param name="dispatchRuntime">The dispatch runtime.</param>
        public void ApplyDispatchBehavior(
            ContractDescription contractDescription,
            ServiceEndpoint endpoint,
            DispatchRuntime dispatchRuntime)
        {
            this.CreateMyDataContractSerializerOperationBehaviors(contractDescription);
        }

        /// <summary>
        /// Validates the specified contract description.
        /// </summary>
        /// <param name="contractDescription">The contract description.</param>
        /// <param name="endpoint">The endpoint.</param>
        public void Validate(ContractDescription contractDescription, ServiceEndpoint endpoint)
        {
        }

        /// <summary>
        /// Creates my data contract serializer operation behaviors.
        /// </summary>
        /// <param name="contractDescription">The contract description.</param>
        internal void CreateMyDataContractSerializerOperationBehaviors(ContractDescription contractDescription)
        {
            foreach (var operation in contractDescription.Operations)
            {
                this.CreateMyDataContractSerializationOperationBehavior(operation);
            }
        }

        /// <summary>
        /// Creates my data contract serialization operation behavior.
        /// </summary>
        /// <param name="operation">The operation.</param>
        private void CreateMyDataContractSerializationOperationBehavior(OperationDescription operation)
        {
            var dataContractSerializerOperationBehavior =
                ((KeyedByTypeCollection<IOperationBehavior>)operation.OperationBehaviors)
                .Find<DataContractSerializerOperationBehavior>();

            dataContractSerializerOperationBehavior.DataContractResolver = this.resolver;
        }
    }
}