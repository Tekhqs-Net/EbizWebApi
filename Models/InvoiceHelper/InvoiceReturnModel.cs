using PaymentGateway;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models.InvoiceHelper
{
    public class InvoiceReturnModel
    {
        public List<Invoice> invoices { get; set; }
    }
}
