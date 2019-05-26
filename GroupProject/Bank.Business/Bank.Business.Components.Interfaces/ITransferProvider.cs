using Bank.MessageTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bank.Business.Components.Interfaces
{
    public interface ITransferProvider
    {
        void Transfer(TransferRequest pTransferRequest);
    }
}
