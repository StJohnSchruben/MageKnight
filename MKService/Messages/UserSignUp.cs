using System;
using Service;
using MKModel;
using System.Collections.ObjectModel;
using ReDefNet;
using System.Runtime.Serialization;
using MKService.Updates;

namespace MKService.Messages
{
    public class UserSignUp : MessageBase
    {
        [DataMember]
        public string UserName { get; set; }
        [DataMember]
        public string Password { get; set; }
        [DataMember]
        public Guid Id { get; set; }
    }
}
