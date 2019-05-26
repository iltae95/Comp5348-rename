using Bank.Business.Components.Model;
using Common;
using Common.Model;

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
