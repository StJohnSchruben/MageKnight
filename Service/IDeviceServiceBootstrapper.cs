

namespace Service
{
    /// <summary>
    /// Represents the service necessities needed at runtime to start up a service.
    /// </summary>
    public interface IDeviceServiceBootstrapper
    {
        /// <summary>
        /// Gets the known type assemblies.
        /// </summary>
        /// <value>The known type assemblies.</value>
        string[] KnownTypeAssemblies { get; }

        /// <summary>
        /// Gets the command handlers.
        /// </summary>
        /// <param name="commandContract">The command contract.</param>
        /// <returns>A list of objects which subscribe to the <see cref="ICommandContract" />.</returns>
        object[] GetCommandHandlers(ICommandContract commandContract);

        /// <summary>
        /// Gets the query handlers.
        /// </summary>
        /// <param name="queryContract">The query contract.</param>
        /// <param name="commandContract">The command contract.</param>
        /// <param name="rootHandler">The root handler.</param>
        /// <returns>
        /// The list of the <see cref="IQueryHandler" /> for the device.
        /// </returns>
        IQueryHandler[] GetQueryHandlers(
            IQueryContract queryContract,
            ICommandContract commandContract,
            IQueryHandler rootHandler);
    }
}