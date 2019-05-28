using BookStore.Business.Entities;
using System;

namespace BookStore.Business.Components.Interfaces
{
    public interface IOrderProvider
    {
        void SubmitOrder(Order pOrder);
        void TransferFundsFailed(Guid pOrderId, Exception pException);
        void TransferFundsComplete(Guid pOrderId);
    }
}
