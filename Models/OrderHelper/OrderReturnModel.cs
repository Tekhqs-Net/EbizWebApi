using PaymentGateway;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models.OrderHelper
{
    public class OrderReturnModel
    {
        public List<SalesOrder> salesOrders { get; set; }
    }
}
