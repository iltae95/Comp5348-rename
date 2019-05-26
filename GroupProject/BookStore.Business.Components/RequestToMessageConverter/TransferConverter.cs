using Common;
using Common.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookStore.Business.Entities.Model;

namespace BookStore.Business.Components.RequestToMessageConverter
{
    public class TransferRequestConverter : IVisitor
    {
        public TransferRequestMessage Result { get; set; }
        public void Visit(IVisitable pVisitable)
        {
            if (pVisitable is TransferRequest)
            {
                var item = pVisitable as TransferRequest;
                Result = new TransferRequestMessage
                {
                    Amount = item.Amount,
                    CustomerId = item.CustomerId,
                    FromAccountNumber = item.FromAccountNumber,
                    ToAccountNumber = item.ToAccountNumber,
                    OrderId = item.OrderId,
                    Topic = item.Topic
                };
            }
        }
    }
}
