using PaymentGateway;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models.TransactionHelper
{
    public class CustomerTransactionModel
    {
        public string paymentMethod { get; set; }
        public string custToken { get; set; }
        public CustomerTransactionRequest customerTransaction { get; set; }
        public TransactionRequestObject transactionRequest { get; set; }
        public Customer customer { get; set; }
        public PaymentMethodProfile paymentMethodProfile { get; set; }
        public Invoice invoice { get; set; }
        public SalesOrder salesOrder { get; set; }
        public bool isOrder { get; set; }
        public SecurityToken securityToken { get; set; }
    }
}
