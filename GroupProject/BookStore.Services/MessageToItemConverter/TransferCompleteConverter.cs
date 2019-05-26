using Common;
using Common.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookStore.Services.MessageTypes.Model;

namespace BookStore.Services.MessageToItemConverter
{
    public class TransferCompleteConverter : IVisitor
    {
        public TransferComplete Result { get; set; }
        public void Visit(IVisitable pVisitable)
        {
            if (pVisitable is TransferCompleteConverter)
            {
                var message = pVisitable as TransferCompleteMessage;
                Result = new TransferComplete
                {
                    OrderGuid = message.OrderGuid
                };
            }
        }
    }
}
