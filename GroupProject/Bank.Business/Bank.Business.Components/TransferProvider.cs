using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bank.Business.Components.Interfaces;
using Bank.Business.Entities;
using System.Transactions;
using System.Data;
using System.Data.Entity.Infrastructure;
using Bank.Services.Interfaces;
using Bank.MessageTypes;
using Bank.Business.Components.Model;
using Bank.Business.Components.PublisherService;
using Bank.Business.Components.ResponseToMessageConverter;

namespace Bank.Business.Components
{
    public class TransferProvider : ITransferProvider
    {


        public void Transfer(TransferRequest pTransferRequest)
        {
            using (TransactionScope lScope = new TransactionScope())
            using (BankEntityModelContainer lContainer = new BankEntityModelContainer())
            {
                try
                {
                    Account fromAcct = lContainer.Accounts.Where(account => pTransferRequest.FromAcctNumber == account.AccountNumber).First(); 
                    Account toAcct = lContainer.Accounts.Where(account => pTransferRequest.ToAcctNumber == account.AccountNumber).First();

                    fromAcct.Withdraw(pTransferRequest.Amount);
                    toAcct.Deposit(pTransferRequest.Amount);

                    var item = new TransferComplete
                    {
                        OrderId = pTransferRequest.OrderId,
                        CustomerId = pTransferRequest.CustomerId
                    };
                    var lVisitor = new TransferCompleteConverter();
                    item.Accept(lVisitor);
                    PublisherServiceClient lClient = new PublisherServiceClient();
                    lClient.Publish(lVisitor.Result);
                    
                    lContainer.SaveChanges();
                    lScope.Complete();
                }
                catch (Exception lException)
                {
                    Console.WriteLine("Error occured while transferring money:  " + lException.Message);

                    var item = new TransferFailed
                    {
                        OrderId = pTransferRequest.OrderId
                    };
                    var lVisitor = new TransferFailedConverter();
                    item.Accept(lVisitor);
                    PublisherServiceClient lClient = new PublisherServiceClient();
                    lClient.Publish(lVisitor.Result);
                    
                }
            }
        }

        private Account GetAccountFromNumber(int pToAcctNumber)
        {
            using (BankEntityModelContainer lContainer = new BankEntityModelContainer())
            {
                return lContainer.Accounts.Where((pAcct) => (pAcct.AccountNumber == pToAcctNumber)).FirstOrDefault();
            }
        }
    }
}
