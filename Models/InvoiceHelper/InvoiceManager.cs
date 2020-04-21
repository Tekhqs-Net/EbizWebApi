using PaymentGateway;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Models.EbizModelHelper;

namespace WebApi.Models.InvoiceHelper
{
    public class InvoiceManager
    {
        //security token defined here
        public async Task<SecurityToken> GetSecurityToken()
        {
            SecurityToken securityToken = new SecurityToken();
            await Task.Run(() =>
            {
                securityToken.SecurityId = "99359f03-b254-4adf-b446-24957fcb46cb";
                securityToken.UserId = "qbouser";
                securityToken.Password = "mW64newLSvu!!!!";
            });
            return securityToken;
        }

        //Add Invoice
        public async Task<dynamic> AddInvoices(Invoice invoice,SecurityToken token)
        {
            var client = new PaymentGateway.IeBizServiceClient();
            var data = await client.AddInvoiceAsync(token, invoice);
            return data;
        }
        //Get Invoice
        public async Task<dynamic> GetInvoiceDetails(InvoiceDetailModel model)
        {
            SecurityToken token = await GetSecurityToken();
            var client = new PaymentGateway.IeBizServiceClient();
            var data = await client.GetInvoiceAsync(token, model.customerId, model.subCustomerId, model.invoiceNo, model.invoiceInternalId);
            return data;
        }
        //Search Invoices
        public async Task<dynamic> SearchAllInvoices(InvoiceDetailModel model)
        {
            SecurityToken token = await GetSecurityToken();
            var client = new PaymentGateway.IeBizServiceClient();
            List<InvoiceReturnModel> invoicesReturnModels = new List<InvoiceReturnModel>();
            int length = 0;
            int lengthnode = 0;
            do
            {
                lengthnode++;
                var data = await client.SearchInvoicesAsync(token, model.customerId, model.subCustomerId, model.invoiceNo, model.invoiceInternalId, model.searchFilters, model.start, model.limit, model.sort, model.includeItems);
                length = data.Length;
                InvoiceReturnModel invoiceReturn = new InvoiceReturnModel();
                invoiceReturn.invoices = data.ToList();
                invoicesReturnModels.Add(invoiceReturn);
                model.start = Int32.Parse(lengthnode + "000");
                model.limit = Int32.Parse(lengthnode + "999");
            } while (length >= 1000);
            return invoicesReturnModels;
        }
        //Search Invoices with Pagination
        public async Task<dynamic> SearchInvoicesWithPagination(InvoiceDetailModel model)
        {
            SecurityToken token = await GetSecurityToken();
            var client = new PaymentGateway.IeBizServiceClient();
            model.start = (model.pageno - 1) * model.pagesize;
            model.limit = model.pagesize;
            var data = await client.SearchInvoicesAsync(token, model.customerId, model.subCustomerId, model.invoiceNo, model.invoiceInternalId, model.searchFilters, model.start, model.limit, model.sort, model.includeItems);
            return data;
        }
    }
}
