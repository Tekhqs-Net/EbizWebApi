using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models.TransactionHelper
{
    public class AVSReturnModel
    {
        public dynamic Result { get; set; }
        public string[] Msgs { get; set; }
        public string AvsResponse { get; set; }
        public string AvsResultCode { get; set; }
        public string CardCodeResult { get; set; }
        public string CardCodeResultCode { get; set; }
        public string CardLevelResult { get; set; }
        public string CardLevelRequestCode { get; set; }
        public string ResultCode { get; set; }
        public string StatusCode { get; set; }
    }
}
