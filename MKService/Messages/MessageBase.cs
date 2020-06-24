

using System.Runtime.Serialization;
using Service;

namespace MKService.Messages
{
    /// <summary>
    /// Base class for all command messages.
    /// </summary>
    public abstract class MessageBase : IMessage
    {
        /// <summary>
        /// Gets or sets the hash used to determine the sender of the message and to prevent feedback
        /// loops.
        /// </summary>
        /// <value>
        /// The origination hash.
        /// </value>
        [DataMember]
        public int OriginationHash { get; set; }
    }
}