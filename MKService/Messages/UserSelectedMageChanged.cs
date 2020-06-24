

using System;
using System.Runtime.Serialization;

namespace MKService.Messages
{
    public class UserSelectedMageChanged : MessageBase
    {
        [DataMember]
        public Guid UserId;
        [DataMember]
        public Guid MageId;
    }
}
