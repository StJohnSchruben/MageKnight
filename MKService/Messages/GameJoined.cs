using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace MKService.Messages
{
    public class GameJoined : MessageBase
    {
        [DataMember]
        public Guid User2Id;

        [DataMember]
        public Guid GameId;
    }
}
