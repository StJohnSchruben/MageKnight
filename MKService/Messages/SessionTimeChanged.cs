
using System;
using System.Runtime.Serialization;

namespace MKService.Messages
{
    [DataContract(Namespace = ServiceConstants.MessageNamespace)]
    public class SessionTimeChanged : MessageBase
    {
        /// <summary>
        /// Gets or sets the new session time.
        /// </summary>
        /// <value>The new session time.</value>
        [DataMember]
        public DateTime NewSessionTime { get; set; }
    }
}