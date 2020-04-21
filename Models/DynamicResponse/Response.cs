using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models.DynamicResponse
{
    public class Response
    {

        public dynamic TransResponse { get; set; }
        public dynamic CustomerResponse { get; set; }
        public dynamic PaymentMethodProfileResponse { get; set; }
        public dynamic SearchTransResponse { get; set; }
        public dynamic AppTransResponse { get; set; }
        public dynamic Invoice { get; set; }
        public dynamic SalesOrder { get; set; }
        public string errorMessage { get; set; }
    }
}
