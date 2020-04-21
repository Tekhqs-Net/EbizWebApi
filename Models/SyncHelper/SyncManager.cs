using PaymentGateway;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Models.CustomerHelper;
using WebApi.Models.OrderHelper;

namespace WebApi.Models.SyncHelper
{

    public class SyncManager
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
        public async Task<dynamic> SyncCustomers(SyncDetailModel model)
        {
            SecurityToken token = await GetSecurityToken();
            var client = new PaymentGateway.IeBizServiceClient();
            List<SyncCustomerReponse> customerReponses = new List<SyncCustomerReponse>();
            SyncResponseModel response = new SyncResponseModel();
            foreach (var item in model.customers)
            {
                SyncCustomerReponse custresponse = new SyncCustomerReponse();
                try
                {
                    var customer = await client.GetCustomerAsync(token, item.CustomerId, "");
                    var updatecust = await new CustomerManager().UpdateCustomer(customer,model.securityToken);
                    custresponse.Status = updatecust.Status;
                    custresponse.Message = "Succesfuly Updated";
                    custresponse.CustomerId = updatecust.CustomerId;
                    custresponse.CustomerInternalId = updatecust.CustomerInternalId;
                    customerReponses.Add(custresponse);
                }
                catch (Exception ex)
                {
                    if (ex.Message.ToString() == "Not Found" && item !=null )
                    {
                        var newcust = await new CustomerManager().AddNewCustomer(item, model.securityToken);
                        custresponse.Status = newcust.Status;
                        custresponse.Message = "Succesfuly Created";
                        custresponse.CustomerId = newcust.CustomerId;
                        custresponse.CustomerInternalId = newcust.CustomerInternalId;
                        customerReponses.Add(custresponse);
                    }
                }
            }
            response.customerReponses = customerReponses;
            return response.customerReponses;
        }
        public async Task<dynamic> SyncOrders(SyncDetailModel model)
        {
            SecurityToken token = await GetSecurityToken();
            var client = new PaymentGateway.IeBizServiceClient();
            List<SyncOrderReponse> orderReponses = new List<SyncOrderReponse>();
            SyncResponseModel response = new SyncResponseModel();
            foreach (var item in model.salesOrders)
            {
                SyncOrderReponse orderreponse = new SyncOrderReponse();
                try
                {
                    var order = await client.GetSalesOrderAsync(token, item.CustomerId, "",item.SalesOrderNumber,"");
                    var updatecust = await new OrderManager().UpdateSalesOrder(token, order);
                    orderreponse.Status = updatecust.Status;
                    orderreponse.Message = "Succesfuly Updated";
                    orderreponse.OrderNo = order.SalesOrderNumber;
                    orderreponse.OrderInternalId = updatecust.SalesOrderInternalId;
                    orderReponses.Add(orderreponse);
                }
                catch (Exception ex)
                {
                    if (ex.Message.ToString() == "Not Found" && item != null)
                    {
                        var neworder = await new OrderManager().AddSalesOrder(token, item);
                        orderreponse.Status = neworder.Status;
                        orderreponse.Message = "Succesfuly Created";
                        orderreponse.OrderNo = item.SalesOrderNumber;
                        orderreponse.OrderInternalId = neworder.SalesOrderInternalId;
                        orderReponses.Add(orderreponse);
                    }
                }
            }
            response.orderReponses = orderReponses;
            return response.orderReponses;
        }
        public async Task<dynamic> SyncInvoices(SyncDetailModel model)
        {
            SyncResponseModel response = new SyncResponseModel();
            return response;
        }
        public async Task<dynamic> SyncItems(SyncDetailModel model)
        {
            SyncResponseModel response = new SyncResponseModel();
            return response;
        }
    }
}
