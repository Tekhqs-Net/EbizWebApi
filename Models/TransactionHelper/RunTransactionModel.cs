using PaymentGateway;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models.TransactionHelper
{
    public class RunTransactionModel
    {
        public TransactionRequestObject transactionRequest { get; set; }
        public ApplicationTransactionRequest applicationTransactionRequest { get; set; }
        public SecurityToken securityToken { get; set; }
    }
}
