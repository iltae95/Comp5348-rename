using Common;
using System;

namespace Bank.Business.Components.Model
{
    class TransferComplete : IVisitable
    {
        public String Topic => "TransferComplete";
        public Guid OrderId { get; set; }
        public int CustomerId { get; set; }

        public void Accept(IVisitor pVisitor)
        {
            pVisitor.Visit(this);
        }
    }
}
