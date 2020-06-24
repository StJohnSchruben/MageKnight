using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace MKService.Messages
{
    
    public class UserAddedMageToArmy : MessageBase
    {
        [DataMember]
        public Guid Id { get; set; }
        [DataMember]
        public Guid UserId { get; set; }
        [DataMember]
        public Guid InstantiatedId { get; set; }
    }
    public class UserAddedMageToInventory : MessageBase
    {
        [DataMember]
        public Guid Id { get; set; }
        [DataMember]
        public Guid UserId { get; set; }
    }
}
