using PaymentGateway;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models.EmailHelper
{
    public class EmailDetailModel
    {
        public SecurityToken securityToken { get; set; }
        public EbizWebForm ebizWebForm { get; set; }
        public string customerId { get; set; }
        public DateTime fromPaymentRequestDateTime { get; set; }
        public DateTime toPaymentRequestDateTime { get; set; }
        public SearchFilter[] searchFilters { get; set; }
        public int start { get; set; }
        public int limit { get; set; }
        public string sort { get; set; }
        public string paymentInternalId { get; set; }
        public Customer customer { get; set; }
        public Invoice invoice { get; set; }
        public SalesOrder salesOrder { get; set; }
    }
}
