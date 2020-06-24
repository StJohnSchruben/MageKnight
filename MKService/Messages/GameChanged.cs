using MKModel;
using System;
using System.Runtime.Serialization;

namespace MKService.Messages
{
    public class GameChanged : MessageBase
    {
        [DataMember]
        public Guid User1Id { get; set; }
        [DataMember]
        public Guid User2Id { get; set; }
        [DataMember]
        public Guid GameId { get; set; }
        [DataMember]
        public int TurnCount { get; set; }
    }
    public class GamesChanged : MessageBase
    {
        [DataMember]
        public Guid Id { get; set; }
    }
}
