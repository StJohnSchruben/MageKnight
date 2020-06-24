using MKModel;
using System;
using System.Runtime.Serialization;


namespace MKService.Messages
{
    public class MageKnightChanged : MessageBase
    {
        [DataMember]
        public IMageKnightModel NewModel { get; set; }
    }
}
