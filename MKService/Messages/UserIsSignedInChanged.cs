using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;

namespace MKService.Messages
{
    public class UserIsSignedInChanged : MessageBase
    {
        [DataMember]
        public Guid UserId { get; set; }
        [DataMember]
        public bool IsSignedIn { get; set; }
    }
}
