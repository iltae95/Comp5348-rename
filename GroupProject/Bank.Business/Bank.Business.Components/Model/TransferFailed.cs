using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank.Business.Components.Model
{
    class TransferFailed : IVisitable
    {
        public String Topic => "TransferFailed";
        public Exception Error { get; set; }
        public int OrderId { get; set; }

        public void Accept(IVisitor pVisitor)
        {
            pVisitor.Visit(this);
        }
    }
}
