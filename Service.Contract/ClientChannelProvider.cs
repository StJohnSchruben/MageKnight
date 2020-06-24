

using System;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Description;
using log4net;


namespace Service.Contract
{
    /// <summary>
    /// A provider of channels for a client of the DVS service.
    /// </summary>
    public sealed class ClientChannelProvider : IClientChannelProvider
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
        /// The logger.
        /// </summary>
        private readonly ILog log = LogManager.GetLogger(typeof(ClientChannelProvider));

        /// <summary>
        /// Initializes a new instance of the <see cref="ClientChannelProvider" /> class.
        /// </summary>
        /// <param name="endpoint">The service endpoint.</param>
        /// <param name="deviceServiceBootstrapper">The device service bootstrapper.</param>
        public ClientChannelProvider(IPEndPoint endpoint, IDeviceServiceBootstrapper deviceServiceBootstrapper)
        {
            if (endpoint == null)
            {
                throw new ArgumentNullException(nameof(endpoint));
            }

            if (deviceServiceBootstrapper == null)
            {
                throw new ArgumentNullException(nameof(deviceServiceBootstrapper));
            }

            this.log.InfoFormat("The service client will look for the WST service at {0}.", endpoint);

            this.endpoint = endpoint;
            this.deviceServiceBootstrapper = deviceServiceBootstrapper;
        }

        /// <summary>
        /// Gets the command channel.
        /// </summary>
        /// <param name="implementation">The client implementation.</param>
        /// <returns>
        /// The command channel.
        /// </returns>
        public ICommandContract GetCommandChannel(object implementation)
        {
            if (implementation == null)
            {
                throw new ArgumentNullException(nameof(implementation));
            }

            this.log.Debug("Creating command contract.");

            var binding = new NetTcpBinding
            {
                Name = "NetTcpBinding_ICommandContract",
                ReceiveTimeout = TimeSpan.MaxValue,
                SendTimeout = TimeSpan.MaxValue,
                OpenTimeout = TimeSpan.MaxValue,
                MaxReceivedMessageSize = int.MaxValue,
                MaxBufferSize = int.MaxValue,
                MaxBufferPoolSize = int.MaxValue,
            };

            var uriBuilder = new UriBuilder
            {
                Host = this.endpoint.Address.ToString(),
                Path = "/Lwi/Wst/Service/Command",
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

            this.log.DebugFormat("The known assemblies are: {0}.", string.Join(", ", assemblies));

            factory.Endpoint.Contract.ContractBehaviors.Add(new KnownTypeContractBehavior(assemblies));

            this.log.Debug("Contract behaviors set.");

            return factory.CreateChannel();
        }

        /// <summary>
        /// Gets the query channel.
        /// </summary>
        /// <param name="implementation">The client implementation.</param>
        /// <returns>
        /// The query channel.
        /// </returns>
        public IQueryContract GetQueryChannel(object implementation)
        {
            if (implementation == null)
            {
                throw new ArgumentNullException(nameof(implementation));
            }

            this.log.Debug("Creating query contract.");

            var binding = new NetTcpBinding
            {
                ReceiveTimeout = TimeSpan.MaxValue,
                SendTimeout = TimeSpan.MaxValue,
                OpenTimeout = TimeSpan.MaxValue,
                MaxReceivedMessageSize = int.MaxValue,
                MaxBufferSize = int.MaxValue,
                MaxBufferPoolSize = int.MaxValue,
            };

            var uriBuilder = new UriBuilder
            {
                Host = this.endpoint.Address.ToString(),
                Path = "/Lwi/Wst/Service/Query",
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

            this.log.DebugFormat("The known assemblies are: {0}.", string.Join(", ", assemblies));

            factory.Endpoint.Contract.ContractBehaviors.Add(new KnownTypeContractBehavior(assemblies));

            this.log.Debug("Contract behaviors set.");

            return factory.CreateChannel();
        }
    }
}