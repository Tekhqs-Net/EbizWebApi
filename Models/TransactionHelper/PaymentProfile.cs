using PaymentGateway;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models.TransactionHelper
{
    public class PaymentProfile
    {
         public string customerToken { get; set; }
         public string paymentMethod { get; set; }
         public PaymentMethodProfile paymentMethodProfile { get; set; }
        public SecurityToken securityToken { get; set; }
    }
}
