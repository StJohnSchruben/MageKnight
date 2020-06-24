



namespace Service.Contract
{
    /// <summary>
    /// Represents a duplex channel provider.
    /// </summary>
    public interface IDuplexChannelProvider
    {
        /// <summary>
        /// Gets the command channel.
        /// </summary>
        /// <param name="implementation">The implementation.</param>
        /// <param name="endpointConfigurationName">Name of the endpoint configuration.</param>
        /// <returns>A client side proxy of <see cref="Service.ICommandContract" />.</returns>
        ICommandContract GetCommandChannel(object implementation, string endpointConfigurationName);

        /// <summary>
        /// Gets the query channel.
        /// </summary>
        /// <param name="implementation">The implementation.</param>
        /// <param name="endpointConfigurationName">Name of the endpoint configuration.</param>
        /// <returns>A client side proxy of <see cref="Service.IQueryContract" />.</returns>
        IQueryContract GetQueryChannel(object implementation, string endpointConfigurationName);
    }
}