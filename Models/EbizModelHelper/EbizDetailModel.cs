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
        public dynamic SearchTransResponse { get; set; }
    }
    public class SearchTransactionDetailModel
    {
        public SearchFilter[] searchFilters { get; set; }
        public bool matchAll { get; set; }
        public bool countOnly { get; set; }
        public string start  { get; set; }
        public string limit { get; set; }
        public string sort { get; set; }
        public SecurityToken token { get; set; }
    }
    public class GetTransactionDetailModel
    {
        public SecurityToken token { get; set; }
        public string transactionRefNum { get; set; }
    }
        public class TransactionReturnModel
    {
        public List<TransactionObject> transactionObjects { get; set; }
    }
    public class CustomerReturnModel
    {
        public List<Customer> customers { get; set; }
    }
    public class OrderDetailModel
    {
        public string customerId { get; set; }
        public string subCustomerId { get; set; }
        public string salesOrderNo { get; set; }
        public string salesOrderInternalId { get; set; }
        public SearchFilter[] searchFilters { get; set; }
        public bool includeItems { get; set; }
        public int start { get; set; }
        public int limit { get; set; }
        public string sort { get; set; }
        public SecurityToken token { get; set; }
}
    public class CustomerDetailModel
    {
        public SecurityToken token { get; set; }
        public string customerId { get; set; }
        public string customerInternalId { get; set; }
        public int start { get; set; }
        public int limit { get; set; }
        public string sort { get; set; }
        public bool includeCustomerToken { get; set; }
        public bool includePaymentMethodProfile { get; set; }
        public bool countOnly { get; set; }
        public SearchFilter[] searchFilters { get; set; }
    }
}
