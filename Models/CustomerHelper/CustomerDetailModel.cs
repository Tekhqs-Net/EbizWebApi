using PaymentGateway;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models.CustomerHelper
{
   public class CustomerDetailModel
    {
        public int pageno { get; set; }
        public int pagesize { get; set; }
        public SecurityToken securityToken { get; set; }
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
