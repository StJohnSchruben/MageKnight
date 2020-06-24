

namespace Service
{
    /// <summary>
    /// Represents a <see cref="IDeviceServiceBootstrapper" /> that can participate in snapshot. This is a separate interface
    /// to prevent breaking components that only implement <see cref="IDeviceServiceBootstrapper" />. Once all components
    /// implement this interface and <see cref="IDeviceServiceBootstrapper" />, both interfaces may be merged.
    /// </summary>
    public interface IDeviceServiceBootstrapperWithSnapshot : IDeviceServiceBootstrapper
    {
        /// <summary>
        /// Gets a snapshot for the model that this bootstrapper manages.
        /// </summary>
        /// <returns>
        /// The snapshot data as a string.
        /// </returns>
        string GetSnapshot();

        /// <summary>
        /// Loads a snapshot of the model that this bootstrapper manages.
        /// </summary>
        /// <param name="snapshot">The snapshot data as a string.</param>
        void LoadSnapshot(string snapshot);
    }
}