

using System;

namespace Service
{
    /// <summary>
    /// Provides data for snapshot-related events.
    /// </summary>
    public class SnapshotEventArgs : EventArgs
    {
        /// <summary>
        /// Gets or sets the path where the snapshot should be loaded from or saved to. This is typically a UNC path and will need
        /// to be converted to a local file path on the file server.
        /// </summary>
        /// <value>
        /// The path from where the snapshot should be loaded from or saved to.
        /// </value>
        public string Path { get; set; }
    }
}