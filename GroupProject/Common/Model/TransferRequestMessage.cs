using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Common.Model
{
    [DataContract]
    public class TransferRequestMessage : Message
    {
        [DataMember]
        public double Amount { get; set; }

        [DataMember]
        public Guid OrderId { get; set; }

        [DataMember]
        public int CustomerId { get; set; }

        [DataMember]
        public int FromAccountNumber { get; set; }

        [DataMember]
        public int ToAccountNumber { get; set; }

    }
}
