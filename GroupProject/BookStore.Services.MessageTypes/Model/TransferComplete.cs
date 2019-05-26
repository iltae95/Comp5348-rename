using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Services.MessageTypes.Model
{
    public class TransferComplete : IVisitable
    {
        public int OrderId { get; set; }

        public void Accept(IVisitor pVisitor)
        {
            pVisitor.Visit(this);
        }
    }
}
