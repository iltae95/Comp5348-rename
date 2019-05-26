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
                    // find the two account entities and add them to the Container
                    Account lFromAcct = lContainer.Accounts.Where(account => pTransferRequest.FromAcctNumber == account.AccountNumber).First(); 
                    Account lToAcct = lContainer.Accounts.Where(account => pTransferRequest.ToAcctNumber == account.AccountNumber).First();

                    // update the two accounts
                    lFromAcct.Withdraw(pTransferRequest.Amount);
                    lToAcct.Deposit(pTransferRequest.Amount);

                    var lItem = new TransferComplete
                    {
                        OrderId = pTransferRequest.OrderId,
                        CustomerId = pTransferRequest.CustomerId
                    };
                    var lVisitor = new TransferCompleteConverter();
                    lItem.Accept(lVisitor);
                    PublisherServiceClient lClient = new PublisherServiceClient();
                    lClient.Publish(lVisitor.Result);

                    // save changed entities and finish the transaction
                    lContainer.SaveChanges();
                    lScope.Complete();
                }
                catch (Exception lException)
                {
                    Console.WriteLine("Error occured while transferring money:  " + lException.Message);

                    var lItem = new TransferFailed
                    {
                        OrderId = pTransferRequest.OrderId
                    };
                    var lVisitor = new TransferFailedConverter();
                    lItem.Accept(lVisitor);
                    PublisherServiceClient lClient = new PublisherServiceClient();
                    lClient.Publish(lVisitor.Result);

                    throw;
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
