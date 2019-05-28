using Common;
using Common.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DeliveryCo.Services.Interfaces;
using BookStore.Services.Interfaces;
using Common.Interfaces;
using BookStore.Services;
using BookStore.Services.MessageToItemConverter;

namespace BookStore.Services
{
    public class SubscriberService : ISubscriberService
    {
        public void PublishToSubscriber(Message pMessage)
        {
            var oService = new OrderService();
            var dnService = new DeliveryNotificationService();
            if (pMessage.GetType() == typeof(TransferCompleteMessage))
            {
                var lMessage = pMessage as TransferCompleteMessage;
                var lVisitor = new TransferCompleteConverter();
                lMessage.Accept(lVisitor);
                oService.TransferFundsComplete(lVisitor.Result);
            }
            else if (pMessage.GetType() == typeof(TransferFailedMessage))
            {
                var lMessage = pMessage as TransferFailedMessage;
                var lVisitor = new TransferFailedConverter();
                lMessage.Accept(lVisitor);
                oService.TransferFundsFailed(lVisitor.Result);
            }
        }
    }
}
