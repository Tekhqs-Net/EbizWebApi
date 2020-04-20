using PaymentGateway;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models.TransactionHelper
{
    public class TransactionReturnModel
    {
        public string AuthOnlyAmount { get; set; }
        public string AuthOnlyCounts { get; set; }
        public string CreditsCount { get; set; }
        public string CreditsAmount { get; set; }
        public string DeclinesAmount { get; set; }
        public string DeclinesCount { get; set; }
        public string ErrorsAmount { get; set; }
        public string ErrorsCount { get; set; }
        public string Limit { get; set; }
        public string SalesAmount { get; set; }
        public string SalesCount { get; set; }
        public string StartIndex { get; set; }
        public string TransactionsMatched { get; set; }
        public string TransactionsReturned { get; set; }
        public string VoidsAmount { get; set; }
        public string VoidsCount { get; set; }
        public List<TransactionObject> transactionObjects { get; set; }
    }
}
