using BookStore.Services.MessageTypes.Model;
using Common;
using Common.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Services.MessageToItemConverter
{
    public class TransferFailedConverter : IVisitor
    {
        public TransferFailed Result { get; set; }
        public void Visit(IVisitable pVisitable)
        {
            if (pVisitable is TransferFailedMessage)
            {
                var message = pVisitable as TransferFailedMessage;
                Result = new TransferFailed
                {
                    Error = message.Error,
                    OrderId = message.OrderId
                };
            }
        }
    }
}
