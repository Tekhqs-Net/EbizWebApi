using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models.SyncHelper
{
    public class SyncResponseModel
    {
        public List<SyncCustomerReponse> customerReponses { get; set; }
        public List<SyncOrderReponse> orderReponses { get; set; }
    }
    public class SyncCustomerReponse
    {
        public string CustomerId { get; set; }
        public string CustomerInternalId { get; set; }
        public string Status { get; set; }
        public string Message { get; set; }
    }
    public class SyncOrderReponse
    {
        public string OrderNo { get; set; }
        public string OrderInternalId { get; set; }
        public string Status { get; set; }
        public string Message { get; set; }
    }

}
