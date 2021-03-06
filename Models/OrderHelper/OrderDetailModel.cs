﻿using PaymentGateway;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models.OrderHelper
{
    public class OrderDetailModel
    {
        public int pageno { get; set; }
        public int pagesize { get; set; }
        public string customerId { get; set; }
        public string subCustomerId { get; set; }
        public string salesOrderNo { get; set; }
        public string salesOrderInternalId { get; set; }
        public SearchFilter[] searchFilters { get; set; }
        public bool includeItems { get; set; }
        public int start { get; set; }
        public int limit { get; set; }
        public string sort { get; set; }
        public SecurityToken securityToken { get; set; }
    }
}
