using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using BookStore.Services.MessageTypes;
using BookStore.Services.MessageTypes.Model;

namespace BookStore.Services.Interfaces
{
    [ServiceContract]
    public interface IOrderService
    {
        [OperationContract]
        [FaultContract(typeof(InsufficientStockFault))]
        void SubmitOrder(Order pOrder);
        void TransferFundsFailed(TransferFailed pItem);
        void TransferFundsComplete(TransferComplete pItem);
    }
}
