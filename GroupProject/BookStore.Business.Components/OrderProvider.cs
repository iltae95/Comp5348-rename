using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BookStore.Business.Components.Interfaces;
using BookStore.Business.Entities;
using System.Transactions;
using Microsoft.Practices.ServiceLocation;
using DeliveryCo.MessageTypes;
using BookStore.Business.Components.RequestToMessageConverter;
using BookStore.Business.Components.PublisherService;
using BookStore.Business.Entities.Model;

namespace BookStore.Business.Components
{
    public class OrderProvider : IOrderProvider
    {
        public IEmailProvider EmailProvider
        {
            get { return ServiceLocator.Current.GetInstance<IEmailProvider>(); }
        }

        public IUserProvider UserProvider
        {
            get { return ServiceLocator.Current.GetInstance<IUserProvider>(); }
        }

        public void SubmitOrder(Entities.Order pOrder)
        {      
            using (TransactionScope lScope = new TransactionScope())
            {

                using (BookStoreEntityModelContainer lContainer = new BookStoreEntityModelContainer())
                {
                    try
                    {
                        pOrder.OrderNumber = Guid.NewGuid();
                        pOrder.Store = "OnLine";
                        foreach (OrderItem lOrderItem in pOrder.OrderItems)
                        {
                            int bookId = lOrderItem.Book.Id;
                            lOrderItem.Book = lContainer.Books.Where(book => bookId == book.Id).First();
                            System.Guid stockId = lOrderItem.Book.Stock.Id;
                            lOrderItem.Book.Stock = lContainer.Stocks.Where(stock => stockId == stock.Id).First();
                        }
                        pOrder.UpdateStockLevels();
                        lContainer.Orders.Add(pOrder);
                        TransferFundsFromCustomer(UserProvider.ReadUserById(pOrder.Customer.Id).BankAccountNumber, pOrder.Total ?? 0.0, pOrder.OrderNumber, pOrder.Customer.Id);
                        
                        PlaceDeliveryForOrder(pOrder);
                        lContainer.SaveChanges();
                        lScope.Complete();                    
                    }
                    catch (Exception lException)
                    {
                        SendOrderErrorMessage(pOrder, lException);
                        IEnumerable<System.Data.Entity.Infrastructure.DbEntityEntry> entries =  lContainer.ChangeTracker.Entries();
                        throw;
                    }
                }
            }
            //SendOrderPlacedConfirmation(pOrder);
        }

        //private void MarkAppropriateUnchangedAssociations(Order pOrder)
        //{
        //    pOrder.Customer.MarkAsUnchanged();
        //    pOrder.Customer.LoginCredential.MarkAsUnchanged();
        //    foreach (OrderItem lOrder in pOrder.OrderItems)
        //    {
        //        lOrder.Book.Stock.MarkAsUnchanged();
        //        lOrder.Book.MarkAsUnchanged();
        //    }
        //}

        private void LoadBookStocks(Order pOrder)
        {
            using (BookStoreEntityModelContainer lContainer = new BookStoreEntityModelContainer())
            {
                foreach (OrderItem lOrderItem in pOrder.OrderItems)
                {
                    lOrderItem.Book.Stock = lContainer.Stocks.Where((pStock) => pStock.Book.Id == lOrderItem.Book.Id).FirstOrDefault();    
                }
            }
        }

        private void SendOrderErrorMessage(Order pOrder, Exception pException)
        {
            EmailServiceRef.EmailServiceClient eClient = new EmailServiceRef.EmailServiceClient();
            eClient.SendEmail(new EmailService.MessageTypes.EmailMessage()
            {
                ToAddresses = pOrder.Customer.Email,
                Message = "There was an error in processsing your order " + pOrder.OrderNumber + ": "+ pException.Message + ". Please contact Book Store"
            });
        }

        private void SendOrderPlacedConfirmation(Order pOrder)
        {
            EmailServiceRef.EmailServiceClient eClient = new EmailServiceRef.EmailServiceClient();
            eClient.SendEmail(new EmailService.MessageTypes.EmailMessage()
            {
                ToAddresses = pOrder.Customer.Email,
                Message = "Your order " + pOrder.OrderNumber + " has been placed#########"

            });
        }

        private void PlaceDeliveryForOrder(Order pOrder)
        {
            DeliveryServiceRef.DeliveryServiceClient deliveryServiceClient = new DeliveryServiceRef.DeliveryServiceClient();
            Delivery lDelivery = new Delivery() { DeliveryStatus = DeliveryStatus.Submitted, SourceAddress = "Book Store Address", DestinationAddress = pOrder.Customer.Address, Order = pOrder };

            //Guid lDeliveryIdentifier = 
            deliveryServiceClient.SubmitDelivery(new DeliveryInfo()
            { 
                OrderNumber = lDelivery.Order.OrderNumber.ToString(),  
                SourceAddress = lDelivery.SourceAddress,
                DestinationAddress = lDelivery.DestinationAddress,
                DeliveryNotificationAddress = "net.msmq://localhost/private/EmailNotifyQueue"
            });

            //lDelivery.ExternalDeliveryIdentifier = lDeliveryIdentifier;
            pOrder.Delivery = lDelivery;   
        }

        private void TransferFundsFromCustomer(int pCustomerAccountNumber, double pTotal, Guid pOrderId, int pCustomerId)
        {
            try
            {
                TransferRequest req = new TransferRequest
                {
                    Amount = pTotal,
                    FromAccountNumber = pCustomerAccountNumber,
                    ToAccountNumber = RetrieveBookStoreAccountNumber(),
                    OrderId = pOrderId,
                    CustomerId = pCustomerId
                };
                TransferRequestConverter lVisitor = new TransferRequestConverter();
                lVisitor.Visit(req);
                PublisherServiceClient lClient = new PublisherServiceClient();
                lClient.Publish(lVisitor.Result);
                //ExternalServiceFactory.Instance.TransferService.Transfer(pTotal, pCustomerAccountNumber, RetrieveBookStoreAccountNumber());
            }
            catch
            {
                throw new Exception("Error when transferring funds for order.");
            }
        }

        public void TransferFundsComplete(Guid pOrderId)
        {
            using (TransactionScope lScope = new TransactionScope())
            {
                using (BookStoreEntityModelContainer lContainer = new BookStoreEntityModelContainer())
                {
                    try
                    {
                        Console.WriteLine("Funds Transfer Complete");
                        var pOrder = lContainer.Orders.Include("Customer").First(x => x.OrderNumber == pOrderId);

                        PlaceDeliveryForOrder(pOrder);

                        lContainer.SaveChanges();
                        lScope.Complete();
                    }
                    catch (Exception lException)
                    {
                        Console.WriteLine("Error in FundsTransferComplete: " + lException.Message);
                        throw;
                    }
                }
            }

        }

        public void TransferFundsFailed(Guid pOrderId)
        {
            using (TransactionScope lScope = new TransactionScope())
            {
                using (BookStoreEntityModelContainer lContainer = new BookStoreEntityModelContainer())
                {
                    try
                    {
                        Console.WriteLine("Funds Transfer Failed");
                        var pOrder = lContainer.Orders
                            .Include("Customer").FirstOrDefault(x => x.OrderNumber == pOrderId);

                        EmailProvider.SendMessage(new EmailMessage()
                        {
                            ToAddress = pOrder.Customer.Email,
                            Message = "There was an error with your credit. The purchase cannot proceed."
                        });
                        lContainer.SaveChanges();
                        lScope.Complete();
                    }
                    catch (Exception lException)
                    {
                        Console.WriteLine("Error in TransferFundsFailed: " + lException.Message);
                        throw;
                    }
                }
            }
        }

        private int RetrieveBookStoreAccountNumber()
        {
            return 123;
        }


    }
}
