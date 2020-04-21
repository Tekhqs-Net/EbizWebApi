using PaymentGateway;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models.SyncHelper
{
    public class SyncDetailModel
    {
        public List<Customer> customers { get; set; }
        public List<SalesOrder> salesOrders { get; set; }
        public List<Invoice> invoices { get; set; }
        public List<Item> items { get; set; }
        public SecurityToken securityToken { get; set; }
    }
}
