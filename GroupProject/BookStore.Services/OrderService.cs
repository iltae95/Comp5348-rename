using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BookStore.Services.Interfaces;
using BookStore.Business.Components.Interfaces;
using Microsoft.Practices.ServiceLocation;
using BookStore.Services.MessageTypes;

using System.ServiceModel;
using BookStore.Services.MessageTypes.Model;

namespace BookStore.Services
{
    public class OrderService : IOrderService
    {

        private IOrderProvider OrderProvider
        {
            get
            {
                return ServiceFactory.GetService<IOrderProvider>();
            }
        }

        public void TransferFundsComplete(TransferComplete pItem)
        {
            OrderProvider.TransferFundsComplete(pItem.OrderGuid);
        }

        public void TransferFundsFailed(TransferFailed pItem)
        {
            OrderProvider.TransferFundsFailed(pItem.OrderGuid);
        }

        public void SubmitOrder(Order pOrder)
        {
            try
            {
                OrderProvider.SubmitOrder(
                    MessageTypeConverter.Instance.Convert<
                    BookStore.Services.MessageTypes.Order,
                    BookStore.Business.Entities.Order>(pOrder)
                );
            }
            catch(BookStore.Business.Entities.InsufficientStockException ise)
            {
                throw new FaultException<InsufficientStockFault>(
                    new InsufficientStockFault() { ItemName = ise.ItemName });
            }
        }
    }
}
