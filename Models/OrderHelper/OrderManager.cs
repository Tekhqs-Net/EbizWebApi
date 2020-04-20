using PaymentGateway;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Models.EbizModelHelper;

namespace WebApi.Models.OrderHelper
{
    public class OrderManager
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
        //Get Order
        public async Task<dynamic> GetOrderDetails(OrderDetailModel model)
        {
            SecurityToken token = await GetSecurityToken();
            var client = new PaymentGateway.IeBizServiceClient();
            var data = await client.GetSalesOrderAsync(token, model.customerId, model.subCustomerId, model.salesOrderNo, model.salesOrderInternalId);
            return data;
        }
        //Search All Orders
        public async Task<dynamic> SearchAllOrders(OrderDetailModel model)
        {
            SecurityToken token = await GetSecurityToken();
            var client = new PaymentGateway.IeBizServiceClient();
            List<OrderReturnModel> ordersReturnModels = new List<OrderReturnModel>();
            int length = 0;
            int lengthnode = 0;
            do
            {
                lengthnode++;
                var data = await client.SearchSalesOrdersAsync(token, model.customerId, model.subCustomerId, model.salesOrderNo, model.salesOrderInternalId, model.searchFilters, model.start, model.limit, model.sort, model.includeItems);
                length = data.Length;
                OrderReturnModel salesOrderReturn = new OrderReturnModel();
                salesOrderReturn.salesOrders = data.ToList();
                ordersReturnModels.Add(salesOrderReturn);
                model.start = Int32.Parse(lengthnode + "000");
                model.limit = Int32.Parse(lengthnode + "999");
            } while (length >= 1000);
            return ordersReturnModels;
        }
        //Search All Orders with Pagination
        public async Task<dynamic> SearchOrdersWithPagination(OrderDetailModel model)
        {
            SecurityToken token = await GetSecurityToken();
            var client = new PaymentGateway.IeBizServiceClient();
            model.start = (model.pageno - 1) * model.pagesize;
            model.limit = model.pagesize;
            var data = await client.SearchSalesOrdersAsync(token, model.customerId, model.subCustomerId, model.salesOrderNo, model.salesOrderInternalId, model.searchFilters, model.start, model.limit, model.sort, model.includeItems);
            return data;
        }
    }
}
