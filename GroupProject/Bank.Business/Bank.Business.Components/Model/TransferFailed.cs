using Common;
using System;

namespace Bank.Business.Components.Model
{
    class TransferFailed : IVisitable
    {
        public String Topic => "TransferFailed";
        public Exception Error { get; set; }
        public Guid OrderId { get; set; }

        public void Accept(IVisitor pVisitor)
        {
            pVisitor.Visit(this);
        }
    }
}
