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
                var res = pVisitable as TransferFailed;
                Result = new TransferFailedMessage
                {
                    Error = res.Error,
                    OrderId = res.OrderId,
                    Topic = res.Topic
                };
            }
        }
    }
}
