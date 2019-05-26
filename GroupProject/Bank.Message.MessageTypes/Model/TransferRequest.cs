using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank.MessageTypes
{
    public class TransferRequest : IVisitable
    {
        public double Amount { get; set; }
        public int FromAcctNumber { get; set; }
        public int ToAcctNumber { get; set; }
        public int OrderGuid { get; set; }
        public int CustomerId { get; set; }

        public void Accept(IVisitor pVisitor)
        {
            pVisitor.Visit(this);
        }
    }
}
