

using System.ServiceModel;


namespace Service.Contract
{
    /// <summary>
    /// A provider of duplex service channels.
    /// </summary>
    /// <seealso cref="Service.Contract.IDuplexChannelProvider" />
    public class DuplexChannelProvider : IDuplexChannelProvider
    {
        /// <summary>
        /// The provider.
        /// </summary>
        private readonly IDeviceServiceBootstrapper provider;

        /// <summary>
        /// Initializes a new instance of the <see cref="DuplexChannelProvider" /> class.
        /// </summary>
        /// <param name="provider">The provider.</param>
        public DuplexChannelProvider(IDeviceServiceBootstrapper provider)
        {
            this.provider = provider;
        }

        /// <summary>
        /// Gets the command channel.
        /// </summary>
        /// <param name="implementation">The implementation.</param>
        /// <param name="endpointConfigurationName">Name of the endpoint configuration.</param>
        /// <returns>
        /// The command channel.
        /// </returns>
        public ICommandContract GetCommandChannel(object implementation, string endpointConfigurationName)
        {
            var factory = new DuplexChannelFactory<ICommandContract>(
                new InstanceContext(implementation),
                endpointConfigurationName);

            var assemblies = this.provider.KnownTypeAssemblies;
            factory.Endpoint.Contract.ContractBehaviors.Add(new KnownTypeContractBehavior(assemblies));

            return factory.CreateChannel();
        }

        /// <summary>
        /// Gets the query channel.
        /// </summary>
        /// <param name="implementation">The implementation.</param>
        /// <param name="endpointConfigurationName">Name of the endpoint configuration.</param>
        /// <returns>
        /// The query channel.
        /// </returns>
        public IQueryContract GetQueryChannel(object implementation, string endpointConfigurationName)
        {
            var factory = new DuplexChannelFactory<IQueryContract>(
                new InstanceContext(implementation),
                endpointConfigurationName);

            var assemblies = this.provider.KnownTypeAssemblies;
            factory.Endpoint.Contract.ContractBehaviors.Add(new KnownTypeContractBehavior(assemblies));

            return factory.CreateChannel();
        }
    }
}