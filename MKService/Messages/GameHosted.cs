
using System;
using System.Runtime.Serialization;

namespace MKService.Messages
{
    public class GameHosted : MessageBase
    {
        [DataMember]
        public Guid User1Id { get; set; }
        
        [DataMember]
        public Guid GameId { get; set; }
    }
}
