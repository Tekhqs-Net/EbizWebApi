using PaymentGateway;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Models.CustomerHelper;
using WebApi.Models.InvoiceHelper;
using WebApi.Models.ItemHelper;
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
                    item.CustomerId = customer.CustomerId;
                    item.CustomerInternalId = customer.CustomerInternalId;
                    var updatecust = await new CustomerManager().UpdateCustomer(item, model.securityToken);
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
                    if (string.IsNullOrEmpty(item.CustomerId))
                    {
                        orderreponse.Status = "Failure";
                        orderreponse.Message = "You must select an existing customer";
                        orderreponse.OrderNo = item.SalesOrderNumber;
                        orderreponse.OrderInternalId ="";
                        orderReponses.Add(orderreponse);
                    }
                    else
                    {
                        var order = await client.GetSalesOrderAsync(token, item.CustomerId, "", item.SalesOrderNumber, "");
                        item.SalesOrderInternalId = order.SalesOrderInternalId;
                        item.SalesOrderNumber = order.SalesOrderNumber;
                        item.CustomerId = order.CustomerId;
                        var updatecust = await new OrderManager().UpdateSalesOrder(token, item);
                        orderreponse.Status = updatecust.Status;
                        orderreponse.Message = "Succesfuly Updated";
                        orderreponse.OrderNo = order.SalesOrderNumber;
                        orderreponse.OrderInternalId = updatecust.SalesOrderInternalId;
                        orderReponses.Add(orderreponse);
                    }
                
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
            SecurityToken token = await GetSecurityToken();
            var client = new PaymentGateway.IeBizServiceClient();
            List<SyncInvoiceReponse> invoiceReponses = new List<SyncInvoiceReponse>();
            SyncResponseModel response = new SyncResponseModel();
            foreach (var item in model.invoices)
            {
                SyncInvoiceReponse invoicereponse = new SyncInvoiceReponse();
                try
                {
                    if (string.IsNullOrEmpty(item.CustomerId))
                    {
                        invoicereponse.Status = "Failure";
                        invoicereponse.Message = "You must select an existing customer";
                        invoicereponse.InvoiceNo = item.InvoiceNumber;
                        invoicereponse.InvoiceInternalId = "";
                        invoiceReponses.Add(invoicereponse);
                    }
                    else
                    {
                        var invoice = await client.GetInvoiceAsync(token, item.CustomerId, "", item.InvoiceNumber, "");
                        item.InvoiceInternalId = invoice.InvoiceInternalId;
                        item.InvoiceNumber = invoice.InvoiceNumber;
                        item.CustomerId = invoice.CustomerId;
                        var updateinvoice = await new InvoiceManager().UpdateInvoices(item, token);
                        invoicereponse.Status = updateinvoice.Status;
                        invoicereponse.Message = "Succesfuly Updated";
                        invoicereponse.InvoiceNo = invoice.InvoiceNumber;
                        invoicereponse.InvoiceInternalId = updateinvoice.InvoiceInternalId;
                        invoiceReponses.Add(invoicereponse);
                    }
                 
                }
                catch (Exception ex)
                {
                    if (ex.Message.ToString() == "Not Found" && item != null)
                    {
                        var newinvoice = await new InvoiceManager().AddInvoices(item,token);
                        invoicereponse.Status = newinvoice.Status;
                        invoicereponse.Message = "Succesfuly Created";
                        invoicereponse.InvoiceNo = item.InvoiceNumber;
                        invoicereponse.InvoiceInternalId = newinvoice.InvoiceInternalId;
                        invoiceReponses.Add(invoicereponse);
                    }
                }
            }
            response.invoiceReponses = invoiceReponses;
            return response.invoiceReponses;
        }
        public async Task<dynamic> SyncItems(SyncDetailModel model)
        {
            SecurityToken token = await GetSecurityToken();
            var client = new PaymentGateway.IeBizServiceClient();
            List<SyncItemReponse> itemReponses = new List<SyncItemReponse>();
            SyncResponseModel response = new SyncResponseModel();
            foreach (var item in model.items)
            {
                SyncItemReponse itemreponse = new SyncItemReponse();
                try
                {
                    var items = await client.SearchItemsAsync(token, "",item.ItemId,null,0,20,"");
                    if (items.Length == 0)
                    {
                        var newitem = await new ItemManager().AddItem(token, item);
                        itemreponse.Status = newitem.Status;
                        itemreponse.Message = "Succesfuly Created";
                        itemreponse.ItemId = item.ItemId;
                        itemreponse.ItemInternalId = newitem.ItemInternalId;
                        itemReponses.Add(itemreponse);
                    }
                    else
                    {
                        item.ItemInternalId = items[0]?.ItemInternalId;
                        item.ItemId = items[0]?.ItemId;
                        var updateitem = await new ItemManager().UpdateItemDetails(token, item);
                        itemreponse.Status = updateitem.Status;
                        itemreponse.Message = "Succesfuly Updated";
                        itemreponse.ItemId = item.ItemId;
                        itemreponse.ItemInternalId = updateitem.ItemInternalId;
                        itemReponses.Add(itemreponse);
                    }
                   
                }
                catch (Exception ex)
                {
                    //if (ex.Message.ToString() == "Not Found" && item != null)
                    //{
                    //    var newitem = await new ItemManager().AddItem(token,item);
                    //    itemreponse.Status = newitem.Status;
                    //    itemreponse.Message = "Succesfuly Created";
                    //    itemreponse.ItemId = item.ItemId;
                    //    itemreponse.ItemInternalId = newitem.InvoiceInternalId;
                    //    itemReponses.Add(itemreponse);
                    //}
                }
            }
            response.syncItemReponses = itemReponses;
            return response.syncItemReponses;
        }
    }
}
