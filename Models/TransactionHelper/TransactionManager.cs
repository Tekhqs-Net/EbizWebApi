using PaymentGateway;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Models.CustomerHelper;
using WebApi.Models.DynamicResponse;
using WebApi.Models.EbizModelHelper;
using WebApi.Models.InvoiceHelper;
using WebApi.Models.OrderHelper;
using WebApi.Models.PaymentHelper;

namespace WebApi.Models.TransactionHelper
{
    public class TransactionManager
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
        //AVS Payment Response
        public async Task<dynamic> AVSPaymentResponse(RunTransactionModel model)
        {
            AVSReturnModel response = new AVSReturnModel();
            //Response response = new Response();
            SecurityToken token = await GetSecurityToken();
            var client = new PaymentGateway.IeBizServiceClient();
            var result = await client.runTransactionAsync(token, model.transactionRequest);
            response.AvsResponse = result.AvsResult;
            response.AvsResultCode = result.AvsResultCode;
            response.CardCodeResult = result.CardCodeResult;
            response.CardCodeResultCode = result.CardCodeResultCode;
            response.CardLevelResult = result.CardLevelResult;
            response.CardLevelRequestCode = result.CardLevelResultCode;
            if (result.AvsResult.Contains("&"))
            {
                response.Msgs = result.AvsResult.Split('&');
                // dosomething...
            }
            if (result.ResultCode == "A")
            {
                model.transactionRequest.RefNum = result.RefNum;
                model.transactionRequest.Command = "void:release";
                var result2 = await client.runTransactionAsync(token, model.transactionRequest);
                if (result2.ResultCode == "A")
                {
                    response.Result = result;
                }
                else
                {
                    response.Result = result;
                }
            }
            else
            {
                response.Result = result;
            }
            return response;
        }
        //New Transaction Logic Here
        public async Task<dynamic> NewTransaction(RunTransactionModel model)
        {
            Response response = new Response();
            SecurityToken token = await GetSecurityToken();
            var client = new PaymentGateway.IeBizServiceClient();
         
            var result = await client.runTransactionAsync(token, model.transactionRequest);
          

            if (result.ResultCode == "A")
            {
                response.TransResponse = result;
            }
            else
            {
                response.TransResponse = result;
            }
            return response;
        }

        //Save Transaction without saving payment details
        public async Task<dynamic> SaveCustTransaction(CustomerTransactionModel model)
        {
            Response response = new Response();
            // if paymentmethod is empty then first add payment method also check customer token as well.
            //if customer token available then search on cstmr token otherwise on id.
            //if paymentmethodid = "" then add and save card details
            // paymentmethodid from model 
            string custIntlId = "";
            string invoiceIntlId = "";
            string orderIntlId = "";
            SecurityToken token = await GetSecurityToken();
            var client = new PaymentGateway.IeBizServiceClient();
            if (!string.IsNullOrEmpty(model.customer?.CustomerId))
            {
                try
                {
                    model.customer = await client.GetCustomerAsync(token, model.customer.CustomerId, "");
                    //getCustomer( by id customer?.CustomerId
                    //assigne to custIntlId = customerInternalID
                    custIntlId = model.customer.CustomerInternalId;
                }
                catch (Exception ex)
                {
                    if (ex.Message.ToString() == "Not Found")
                    {
                        custIntlId = "";
                    }
                }
            }
            if (string.IsNullOrEmpty(custIntlId.ToString()) && model.customer != null)
            {
                //Add customer
                response.CustomerResponse = await new CustomerManager().AddNewCustomer(model.customer, token);
                //get customertoken
                model.customer.CustomerToken = await client.GetCustomerTokenAsync(token, model.customer.CustomerId, model.customer.CustomerInternalId);
                custIntlId = response.CustomerResponse.CustomerInternalId;
            }
            if (string.IsNullOrEmpty(model.paymentMethod) && model.paymentMethodProfile != null)
            {
                //Add Payment method (use custIntlId
                //if secondrySort == 0 then call SetAsDefault
                // values must come from controller.
                response.PaymentMethodProfileResponse = await new PaymentManager().AddNewPaymentDetails(model.customer.CustomerInternalId, model.paymentMethodProfile);
                model.paymentMethod = response.PaymentMethodProfileResponse;
                // this logic also implemented in controller.
                if (model.paymentMethodProfile.SecondarySort == "0")
                {
                    PaymentProfile obj = new PaymentProfile();
                    obj.customerToken = model.customer.CustomerToken;
                    obj.paymentMethod = response.PaymentMethodProfileResponse;
                    obj.securityToken = token;
                    var status = await new PaymentManager().SetDefaultPaymentMethodProfile(obj);
                }
            }
            if (!string.IsNullOrEmpty(model.customerTransaction?.Details?.Invoice) && model.isOrder == false)
            {
                try
                {
                    model.invoice = await client.GetInvoiceAsync(token, model.customer.CustomerId, "",model.customerTransaction.Details.Invoice,"");
                    invoiceIntlId = model.invoice.InvoiceInternalId;
                }
                catch (Exception ex)
                {
                    if (ex.Message.ToString() == "Not Found")
                    {
                        invoiceIntlId = "";
                    }
                }
            }
            if (string.IsNullOrEmpty(invoiceIntlId.ToString()) && model.invoice != null && model.isOrder == false)
            {
                //Add new Invoice
                response.Invoice = await new InvoiceManager().AddInvoices(model.invoice,token);
                invoiceIntlId = response.Invoice.InvoiceInternalId;
            }
            if (!string.IsNullOrEmpty(model.customerTransaction?.Details?.OrderID) && model.isOrder == true)
            {
                try
                {
                    model.salesOrder = await client.GetSalesOrderAsync(token, model.customer.CustomerId, model.customer.CustomerInternalId, model.customerTransaction.Details.OrderID, "");
                    orderIntlId = model.salesOrder.SalesOrderInternalId;
                }
                catch (Exception ex)
                {
                    if (ex.Message.ToString() == "Not Found")
                    {
                        orderIntlId = "";
                    }
                }
            }
            if (string.IsNullOrEmpty(orderIntlId.ToString()) && model.salesOrder != null && model.isOrder == true)
            {
                //Add new Invoice
                response.SalesOrder = await new OrderManager().AddSalesOrder(token, model.salesOrder);
                orderIntlId = response.SalesOrder.SalesOrderInternalId;
            }

            //Call Run Customer transaction
            ///
            //end
            var result = await client.runCustomerTransactionAsync(token, model.customer.CustomerToken, model.paymentMethod, model.customerTransaction);
            if (model.isOrder == true)
            {
                if (!string.IsNullOrEmpty(model.customerTransaction.Details.OrderID))
                {
                    ApplicationTransactionRequest obj = new ApplicationTransactionRequest();
                    {
                        obj.CustomerInternalId = custIntlId;
                        obj.LinkedToInternalId = orderIntlId;//sales order internal id or invoice internal id 
                        obj.LinkedToTypeId = model.customerTransaction.Details.OrderID;//sales order number or invoice number
                        obj.TransactionId = result.RefNum;
                        obj.TransactionDate = DateTime.Now.ToString();
                        obj.TransactionTypeId = "AuthOnly";
                        obj.SoftwareId = ".NetApi";
                    }
                    response.AppTransResponse = await client.AddApplicationTransactionAsync(token, obj);
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(model.customerTransaction.Details.Invoice))
                {
                    ApplicationTransactionRequest obj = new ApplicationTransactionRequest();
                    {
                        obj.CustomerInternalId = custIntlId;
                        obj.LinkedToInternalId = invoiceIntlId;//sales order internal id or invoice internal id 
                        obj.LinkedToTypeId = model.customerTransaction.Details.Invoice;//sales order number or invoice number
                        obj.TransactionId = result.RefNum;
                        obj.TransactionDate = DateTime.Now.ToString();
                        obj.TransactionTypeId = "AuthOnly";
                        obj.SoftwareId = ".NetApi";
                    }
                    response.AppTransResponse = await client.AddApplicationTransactionAsync(token, obj);
                }
            }
                
        
            if (result.ResultCode == "A")
            {
                response.TransResponse = result;
            }
            else
            {
                response.TransResponse = result;
            }
            return response;
        }
        //Get Transaction
        public async Task<dynamic> GetTransactionDetails(TransactionDetailModel model)
        {
            SecurityToken token = await GetSecurityToken();
            var client = new PaymentGateway.IeBizServiceClient();
            var data = await client.GetTransactionDetailsAsync(token, model.transactionRefNum);
            return data;
        }
        //Search All Transaction
        public async Task<dynamic> SearchAllTransactionDetails(TransactionDetailModel model)
        {
            List<TransactionReturnModel> transactionReturnModels = new List<TransactionReturnModel>();
            SecurityToken token = await GetSecurityToken();
            var client = new PaymentGateway.IeBizServiceClient();
            int length = 0;
            int lengthnode = 0;
            do
            {
                lengthnode++;
                var data = await client.SearchTransactionsAsync(token, model.searchFilters, model.matchAll, model.countOnly, model.start, model.limit, model.sort);
                length = Int32.Parse(data.TransactionsReturned);
                TransactionReturnModel transactionReturn = new TransactionReturnModel();
                transactionReturn.transactionObjects = data.Transactions.ToList();
                transactionReturn.TransactionsMatched = data.TransactionsMatched;
                transactionReturn.TransactionsReturned = data.TransactionsReturned;
                transactionReturn.AuthOnlyAmount = data.AuthOnlyAmount.ToString();
                transactionReturn.AuthOnlyCounts = data.AuthOnlyCount.ToString();
                transactionReturn.CreditsAmount = data.CreditsAmount.ToString();
                transactionReturn.CreditsCount = data.CreditsCount;
                transactionReturn.DeclinesAmount = data.DeclinesAmount.ToString();
                transactionReturn.DeclinesCount = data.DeclinesCount;
                transactionReturn.ErrorsAmount = data.ErrorsAmount.ToString();
                transactionReturn.ErrorsCount = data.ErrorsCount;
                transactionReturn.SalesAmount = data.SalesAmount.ToString();
                transactionReturn.SalesCount = data.SalesCount;
                transactionReturn.VoidsAmount = data.VoidsAmount.ToString();
                transactionReturn.VoidsCount = data.VoidsCount;
                transactionReturn.StartIndex = data.StartIndex;
                transactionReturn.Limit = data.Limit;
                transactionReturnModels.Add(transactionReturn);
                model.start = lengthnode + "000";
                model.limit = lengthnode + "999";
            } while (length >= 1000);

            return transactionReturnModels;
        }
        //Search Transaction with pagination
        public async Task<dynamic> SearchTransactionWithPagination(TransactionDetailModel model)
        {
            List<TransactionReturnModel> transactionReturnModels = new List<TransactionReturnModel>();
            SecurityToken token = await GetSecurityToken();
            var client = new PaymentGateway.IeBizServiceClient();
            int strt = (model.pageno - 1) * model.pagesize;
            int lmt = model.pagesize;
            var data = await client.SearchTransactionsAsync(token, model.searchFilters, model.matchAll, model.countOnly, strt.ToString(), lmt.ToString(), model.sort);
            return data;
        }
    }
}
