using Service;
using Service.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;
using System.Threading.Tasks;

namespace MKService
{
    internal class ClientChannelProvider : IClientChannelProvider
    {
        /// <summary>
        /// The device service bootstrapper.
        /// </summary>
        private readonly IDeviceServiceBootstrapper deviceServiceBootstrapper;

        /// <summary>
        /// The service endpoint.
        /// </summary>
        private readonly IPEndPoint endpoint;

        /// <summary>
        /// Initializes a new instance of the <see cref="ClientChannelProvider" /> class.
        /// </summary>
        /// <param name="endpoint">The service endpoint.</param>
        /// <param name="deviceServiceBootstrapper">The device service bootstrapper.</param>
        public ClientChannelProvider(IPEndPoint endpoint, IDeviceServiceBootstrapper deviceServiceBootstrapper)
        {

            Console.WriteLine($"The service client will look for the WST service at {endpoint}.");
            this.endpoint = endpoint;
            this.deviceServiceBootstrapper = deviceServiceBootstrapper;
        }

        /// <summary>
        /// Gets the command channel.
        /// </summary>
        /// <param name="implementation">The client implementation.</param>
        /// <returns>The command channel.</returns>
        public ICommandContract GetCommandChannel(object implementation)
        {
            var binding = new NetTcpBinding
            {
                Name = "NetTcpBinding_ICommandContract",
                TransferMode = TransferMode.Buffered,
                ReceiveTimeout = TimeSpan.MaxValue,
                SendTimeout = TimeSpan.MaxValue,
                OpenTimeout = TimeSpan.MaxValue,
                MaxReceivedMessageSize = int.MaxValue,
                MaxBufferSize = int.MaxValue,
                MaxBufferPoolSize = int.MaxValue,
            };

            binding.ReliableSession.InactivityTimeout = TimeSpan.MaxValue;

            var uriBuilder = new UriBuilder
            {
                Host = this.endpoint.Address.ToString(),
                Path = "/MageKnight2D/Service/Command",
                Port = this.endpoint.Port,
                Scheme = "net.tcp"
            };

            var endpointAddress = new EndpointAddress(uriBuilder.Uri);

            var contractDescription = ContractDescription.GetContract(typeof(ICommandContract));

            var serviceEndpoint = new ServiceEndpoint(contractDescription, binding, endpointAddress);

            var instanceContext = new InstanceContext(implementation);

            var factory = new DuplexChannelFactory<ICommandContract>(instanceContext, serviceEndpoint);

            foreach (var operation in factory.Endpoint.Contract.Operations)
            {
                var dataContractSerializerBehavior =
                    operation.Behaviors.Find<DataContractSerializerOperationBehavior>();

                if (dataContractSerializerBehavior != null)
                {
                    dataContractSerializerBehavior.MaxItemsInObjectGraph = int.MaxValue;
                }
            }

            var assemblies = this.deviceServiceBootstrapper.KnownTypeAssemblies;
            factory.Endpoint.Contract.ContractBehaviors.Add(new KnownTypeContractBehavior(assemblies));

            return factory.CreateChannel();
        }

        /// <summary>
        /// Gets the query channel.
        /// </summary>
        /// <param name="implementation">The client implementation.</param>
        /// <returns>The query channel.</returns>
        public IQueryContract GetQueryChannel(object implementation)
        {

            var binding = new NetTcpBinding
            {
                TransferMode = TransferMode.Buffered,
                ReceiveTimeout = TimeSpan.MaxValue,
                SendTimeout = TimeSpan.MaxValue,
                OpenTimeout = TimeSpan.MaxValue,
                MaxReceivedMessageSize = int.MaxValue,
                MaxBufferSize = int.MaxValue,
                MaxBufferPoolSize = int.MaxValue,
            };

            binding.ReliableSession.InactivityTimeout = TimeSpan.MaxValue;

            var uriBuilder = new UriBuilder
            {
                Host = this.endpoint.Address.ToString(),
                Path = "/MageKnight2D/Service/Query",
                Port = this.endpoint.Port,
                Scheme = "net.tcp"
            };

            var endpointAddress = new EndpointAddress(uriBuilder.Uri);

            var contractDescription = ContractDescription.GetContract(typeof(IQueryContract));

            var serviceEndpoint = new ServiceEndpoint(contractDescription, binding, endpointAddress);

            var factory = new DuplexChannelFactory<IQueryContract>(
                new InstanceContext(implementation),
                serviceEndpoint);

            foreach (var operation in factory.Endpoint.Contract.Operations)
            {
                var dataContractSerializerBehavior =
                    operation.Behaviors.Find<DataContractSerializerOperationBehavior>();

                if (dataContractSerializerBehavior != null)
                {
                    dataContractSerializerBehavior.MaxItemsInObjectGraph = int.MaxValue;
                }
            }

            var assemblies = this.deviceServiceBootstrapper.KnownTypeAssemblies;
            factory.Endpoint.Contract.ContractBehaviors.Add(new KnownTypeContractBehavior(assemblies));

            return factory.CreateChannel();
        }
    }
}
