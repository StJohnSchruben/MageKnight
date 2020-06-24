

using System;

namespace Service
{
    /// <summary>
    /// Event data for a published command message.
    /// </summary>
    public class PublishedEventArgs : EventArgs
    {
        /// <summary>
        /// Gets or sets the command message.
        /// </summary>
        /// <value>
        /// The command message.
        /// </value>
        public IMessage Message { get; set; }
    }
}