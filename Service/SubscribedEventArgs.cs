

using System;

namespace Service
{
    /// <summary>
    /// Event data for a command message subscription.
    /// </summary>
    public class SubscribedEventArgs : EventArgs
    {
        /// <summary>
        /// Gets or sets the command callback.
        /// </summary>
        /// <value>
        /// The command callback.
        /// </value>
        public ICommandCallbackContract Callback { get; set; }

        /// <summary>
        /// Gets or sets the command message type.
        /// </summary>
        /// <value>
        /// The command message type.
        /// </value>
        public string MessageType { get; set; }
    }
}