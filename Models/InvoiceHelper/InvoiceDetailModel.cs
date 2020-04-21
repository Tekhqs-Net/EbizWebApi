using PaymentGateway;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models.InvoiceHelper
{
    public class InvoiceDetailModel
    {
        public Invoice invoice { get; set; }
        public int pageno { get; set; }
        public int pagesize { get; set; }
        public string customerId { get; set; }
        public string subCustomerId { get; set; }
        public string invoiceNo { get; set; }
        public string invoiceInternalId { get; set; }
        public SearchFilter[] searchFilters { get; set; }
        public bool includeItems { get; set; }
        public int start { get; set; }
        public int limit { get; set; }
        public string sort { get; set; }
        public SecurityToken securityToken { get; set; }
    }
}
