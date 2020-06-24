using System;
using Service;
using MKModel;
using System.Collections.ObjectModel;
using ReDefNet;
using System.Runtime.Serialization;
using MKService.Updates;

namespace MKService.Messages
{
    public class UserBoosterPackCountChanged : MessageBase
    {
        [DataMember]
        public int Count { get; set; }
        [DataMember]
        public BoosterPack Set { get; set; }
        [DataMember]
        public Guid UserId { get; set; }
    }
}
