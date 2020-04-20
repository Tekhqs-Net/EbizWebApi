using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models.EmailHelper
{
    public class EmailResponse
    {
        public dynamic Customer { get; set; }
        public dynamic Invoice { get; set; }
        public dynamic SalesOrder { get; set; }
        public string EbizWebFormLink { get; set; }
    }
}
