

namespace Service
{
    /// <summary>
    /// Represents a command message, which typically represents some system state change.
    /// </summary>
    public interface IMessage
    {
        /// <summary>
        /// Gets or sets the hash used to determine the sender of the message and to prevent feedback loops.
        /// </summary>
        /// <value>
        /// The origination hash.
        /// </value>
        int OriginationHash { get; set; }
    }
}