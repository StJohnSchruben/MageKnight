



namespace Service.Contract
{
    /// <summary>
    /// Represents a provider of channels for a client of the DVS service.
    /// </summary>
    public interface IClientChannelProvider
    {
        /// <summary>
        /// Gets the command channel.
        /// </summary>
        /// <param name="implementation">The client implementation.</param>
        /// <returns>
        /// The command channel.
        /// </returns>
        ICommandContract GetCommandChannel(object implementation);

        /// <summary>
        /// Gets the query channel.
        /// </summary>
        /// <param name="implementation">The client implementation.</param>
        /// <returns>
        /// The query channel.
        /// </returns>
        IQueryContract GetQueryChannel(object implementation);
    }
}