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
    public class TransferFailedConverter : IVisitor
    {
        public TransferFailedMessage Result { get; set; }
        public void Visit(IVisitable pVisitable)
        {
            if (pVisitable is TransferFailed)
            {
                var lItem = pVisitable as TransferFailed;
                Result = new TransferFailedMessage
                {
                    Error = lItem.Error,
                    OrderId = lItem.OrderId,
                    Topic = lItem.Topic
                };
            }
        }
    }
}
