

namespace ReDefNet
{
    /// <summary>
    /// The initialization status of a component.
    /// </summary>
    public enum InitializationStatus
    {
        /// <summary>
        /// The component is destroyed.
        /// </summary>
        Destroyed,

        /// <summary>
        /// The component is transitioning to the initialized state.
        /// </summary>
        Initializing,

        /// <summary>
        /// The component is initialized.
        /// </summary>
        Initialized,

        /// <summary>
        /// The component is transitioning to the destroyed state.
        /// </summary>
        Destroying
    }
}