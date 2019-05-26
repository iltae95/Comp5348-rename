using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Common.Model
{
    [DataContract]
    public class TransferCompleteMessage : Message
    {
        [DataMember]
        public Guid OrderId { get; set; }

        [DataMember]
        public int CustomerId { get; set; }
    }
}
