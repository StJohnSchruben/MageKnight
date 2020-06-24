using System;
using System.Runtime.Serialization;

namespace MKService.Messages
{
    public class MageKnightCoordinatesChanged :MessageBase
    {
        [DataMember]
        public Guid UserId { get; set; }
        [DataMember]
        public Guid InstantiatedMageId { get; set; }
        [DataMember]
        public double XCoordinate { get; set;}
        [DataMember]
        public double YCoordinate { get; set; }
    }
}
