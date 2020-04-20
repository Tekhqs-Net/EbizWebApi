using PaymentGateway;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models.TransactionHelper
{
    public class TransactionDetailModel
    {
        public int pageno { get; set; }
        public int pagesize { get; set; }
        public SearchFilter[] searchFilters { get; set; }
        public bool matchAll { get; set; }
        public bool countOnly { get; set; }
        public string start { get; set; }
        public string limit { get; set; }
        public string sort { get; set; }
        public string transactionRefNum { get; set; }
        public SecurityToken securityToken { get; set; }
    }
}
