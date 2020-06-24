

using System;

namespace Service
{
    /// <summary>
    /// Represents a special <see cref="IDeviceServiceBootstrapper" /> that translates received Block II service commands
    /// related to snapshot into events that can be listened to by the Block II service. Only one of these should ever exist
    /// within the entire Block II system.
    /// </summary>
    public interface ISnapshotAdapterDeviceServiceBootstrapper : IDeviceServiceBootstrapper
    {
        /// <summary>
        /// Occurs when a request to create a snapshot is received.
        /// </summary>
        event EventHandler<SnapshotEventArgs> CreateSnapshotRequested;

        /// <summary>
        /// Occurs when a request to load a snapshot is received.
        /// </summary>
        event EventHandler<SnapshotEventArgs> LoadSnapshotRequested;
    }
}