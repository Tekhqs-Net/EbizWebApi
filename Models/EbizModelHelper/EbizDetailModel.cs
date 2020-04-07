using PaymentGateway;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Models.ModelHelper;

namespace WebApi.Models.EbizModelHelper
{
    public class EbizRunTranModel
    {
        public TransactionRequestObject transactionRequest { get; set; }
        //public CustomerTransactionRequest customerTransaction { get; set; }
        //public Customer customer { get; set; }
        //public PaymentMethodProfile paymentMethodProfile { get; set; }
        public SecurityToken token { get; set; }
    }
    public class EbizCustTranModel
    {
        public string paymentMethod { get; set; }
        public string custToken { get; set; }
        public CustomerTransactionRequest customerTransaction { get; set; }
        public Customer customer { get; set; }
        public PaymentMethodProfile  paymentMethodProfile { get; set; }
        public SecurityToken token { get; set; }
    }
    public class EbizPaymentProfile
    {
        public string customerToken { get; set; }
        public string paymentMethod { get; set; }
        public SecurityToken token { get; set; }
    }
    public class EbizUpdatePaymentProfile
    {
        public string customerToken { get; set; }
        public PaymentMethodProfile paymentMethod { get; set; }
        public SecurityToken token { get; set; }
    }
    public class Response
    {
        public dynamic TransResponse { get; set; }
        public dynamic CustomerResponse { get; set; }
        public dynamic PaymentMethodProfileResponse { get; set; }
    }
}
