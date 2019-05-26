using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Business.Entities.Model
{
    public class TransferRequest : IVisitable
    {
        public String Topic => "TransferRequest";
        public double Amount { get; set; }
        public int FromAccountNumber { get; set; }
        public int ToAccountNumber { get; set; }
        public int OrderId { get; set; }
        public int CustomerId { get; set; }

        public void Accept(IVisitor pVisitor)
        {
            pVisitor.Visit(this);
        }
    }
}
