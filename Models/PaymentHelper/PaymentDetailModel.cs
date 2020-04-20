using PaymentGateway;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models.PaymentHelper
{
    public class PaymentDetailModel
    {
        public string customerToken { get; set; }
        public string customerId { get; set; }
        public string customerInternalId { get; set; }
        public DateTime fromDateTime { get; set; }
        public DateTime toDateTime { get; set; }
        public int start { get; set; }
        public int limit { get; set; }
        public string sort { get; set; }
        public string invoiceNumber { get; set; }
        public string paymentInternalId { get; set; }
        public string paymentMethod { get; set; }
        public PaymentMethodProfile paymentMethodProfile { get; set; }
        public SecurityToken securityToken { get; set; }
    }
}
