using MKModel;
using MKService.Updates;
using System;
using System.Runtime.Serialization;


namespace MKService.Messages
{
    public class UserChanged : MessageBase
    {
        [DataMember]
        public Guid Id { get; set; }
        //public IUserModel NewModel { get; set; }
    }

    public class UserInventoryAdd : MessageBase
    {
        [DataMember]
        public Guid Id { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public int Index { get; set; }
        [DataMember]
        public int Range { get; set; }
        [DataMember]
        public int PointValue { get; set; }
        [DataMember]
        public int FrontArc { get; set; }
        [DataMember]
        public int Targets { get; set; }
        [DataMember]
        public int Click { get; set; }
        [DataMember]
        public string Set { get; set; }
        [DataMember]
        public string Faction { get; set; }
        [DataMember]
        public string Rank { get; set; }
        [DataMember]
        public byte[] ModelImage { get; set; }
        [DataMember]
        public IDial Dial { get; set; }
    }

    public class UserArmyAdd : MessageBase
    {
        [DataMember]
        public Guid Id { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public int Index { get; set; }
        [DataMember]
        public int Range { get; set; }
        [DataMember]
        public int PointValue { get; set; }
        [DataMember]
        public int FrontArc { get; set; }
        [DataMember]
        public int Targets { get; set; }
        [DataMember]
        public int Click { get; set; }
        [DataMember]
        public string Set { get; set; }
        [DataMember]
        public string Faction { get; set; }
        [DataMember]
        public string Rank { get; set; }
        [DataMember]
        public byte[] ModelImage { get; set; }
        [DataMember]
        public IDial Dial { get; set; }
    }
}
