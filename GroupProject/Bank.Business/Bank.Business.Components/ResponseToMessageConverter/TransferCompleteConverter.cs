using Bank.Business.Components.Model;
using Common;
using Common.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank.Business.Components.ResponseToMessageConverter
{
    public class TransferCompleteConverter : IVisitor
    {
        public TransferCompleteMessage Result { get; set; }
        public void Visit(IVisitable pVisitable)
        {
            if (pVisitable is TransferComplete)
            {
                var res = pVisitable as TransferComplete;
                Result = new TransferCompleteMessage
                {
                    CustomerId = res.CustomerId,
                    OrderId = res.OrderId,
                    Topic = res.Topic
                };
            }
        }
    }
}
