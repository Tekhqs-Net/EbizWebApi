using Newtonsoft.Json;
using PaymentGateway;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Models.EbizModelHelper;

namespace WebApi.Models.ServiceHelper
{
    public class ServiceHandler
    {
        //security token defined here
        public  async Task<SecurityToken> GetSecurityToken()
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

        //New Transaction Logic Here
        public async Task<dynamic> NewTransaction(EbizRunTranModel model)
        {
            Response response = new Response();
            SecurityToken token = await GetSecurityToken();
            var client = new PaymentGateway.IeBizServiceClient();
            var result = await client.runTransactionAsync(token,model.transactionRequest);
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
        public async Task<dynamic> SaveCustTransaction(EbizCustTranModel model)
        {
            Response response = new Response();
            // if paymentmethod is empty then first add payment method also check customer token as well.
            //if customer token available then search on cstmr token otherwise on id.
            //if paymentmethodid = "" then add and save card details
            // paymentmethodid from model 
            string custIntlId = "";
            SecurityToken token = await GetSecurityToken();
            var client = new PaymentGateway.IeBizServiceClient();
            if (string.IsNullOrWhiteSpace(model.customer?.CustomerInternalId))
            {
                try
                {
                    model.customer = await client.GetCustomerAsync(token,model.customer.CustomerId,model.customer.CustomerInternalId);
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

            if (string.IsNullOrWhiteSpace(custIntlId.ToString()) && model.customer != null)
            {
                //Add customer
                // custIntlId = customerIntlId
                response.CustomerResponse = await new ServiceHandler().AddNewCustomer(model.customer);
                //get customertoken
                model.customer.CustomerToken = await client.GetCustomerTokenAsync(token, model.customer.CustomerId, model.customer.CustomerInternalId);
                custIntlId = response.CustomerResponse.CustomerInternalId;
            }
            if (string.IsNullOrWhiteSpace(model.paymentMethod) && model.paymentMethodProfile != null)
            {
                //Add Payment method (use custIntlId
                //if secondrySort == 0 then call SetAsDefault
                // values must come from controller.
                response.PaymentMethodProfileResponse = await new ServiceHandler().AddNewPaymentDetails(model.customer.CustomerInternalId, model.paymentMethodProfile) ;
                model.paymentMethod = response.PaymentMethodProfileResponse;
                // this logic also implemented in controller.
                if (model.paymentMethodProfile.SecondarySort == "0")
                {
                    EbizPaymentProfile obj = new EbizPaymentProfile();
                    obj.customerToken = model.customer.CustomerToken;
                    obj.paymentMethod = response.PaymentMethodProfileResponse;
                    obj.token = token;
                    var status = await new ServiceHandler().SetDefaultPaymentMethodProfile(obj);
                }
            }

            //Call Run Customer transaction
            ///
            //end
            var result = await client.runCustomerTransactionAsync(token, model.customer.CustomerToken, model.paymentMethod, model.customerTransaction);
            if (result.ResultCode == "A")
            {
                response.TransResponse =  result;
            }
            else
            {
                response.TransResponse = result;
            }
            return response;
        }
        //Save Transaction with saving payment details
        //public async Task<string> SaveTransactionWithCard(EbizDetailModel model)
        //{
        //    string msg = "";
        //    SecurityToken token = await GetSecurityToken();
        //    var client = new PaymentGateway.IeBizServiceClient();
        //    Customer customer = await client.GetCustomerAsync(token, "409", "");
        //    PaymentMethodProfile payMethod = new PaymentMethodProfile();
        //    payMethod.CardExpiration = "0922";
        //    payMethod.CardNumber = "4000100511112229";
        //    payMethod.AvsStreet = "123 Main st.";
        //    payMethod.AvsZip = "90046";
        //    payMethod.CardCode = "999";
        //    payMethod.MethodName = "My Visa";
        //    string res = await AddNewPaymentDetails(customer.CustomerInternalId, payMethod);
        //    string paymentmethod = "0";
        //    var response = await client.runCustomerTransactionAsync(token, customer.CustomerId,paymentmethod, model.customerTransaction);
        //    if (response.ResultCode == "A")
        //    {
        //        msg = "Transaction Approved, RefNum: " + response.RefNum;
        //    }
        //    else
        //    {
        //        msg = "Transaction Failed: " + response.Error;
        //    }
        //    return msg;
        //}
        //AddNew PAyment Card Details
        //Delete PaymentMethod Profile
        public async Task<dynamic> DeletePaymentMethodProfile(EbizPaymentProfile model)
        {
            Response response = new Response();
            SecurityToken token = await GetSecurityToken();
            var client = new PaymentGateway.IeBizServiceClient();
            var result = await client.DeleteCustomerPaymentMethodProfileAsync(token, model.customerToken, model.paymentMethod);
            return result;
        }
        //Get Transaction
        public async Task<dynamic> GetTransactionDetails(GetTransactionDetailModel model)
        {
            List<TransactionReturnModel> transactionReturnModels = new List<TransactionReturnModel>();
            SecurityToken token = await GetSecurityToken();
            var client = new PaymentGateway.IeBizServiceClient();
            var data = await client.GetTransactionDetailsAsync(token, model.transactionRefNum);
            return data;
        }
        //Search All Transaction
        public async Task<dynamic> SearchAllTransactionDetails(SearchTransactionDetailModel model)
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
                //TransactionObject[] trans = new TransactionObject[length];
                transactionReturn.transactionObjects = data.Transactions.ToList();
                transactionReturnModels.Add(transactionReturn);
                model.start = lengthnode + "000";
                model.limit = lengthnode + "999";
            } while (length >= 1000);
            return transactionReturnModels;
        }
        //Search Transaction with pagination
        public async Task<dynamic> SearchTransactionWithPagination(SearchTransactionDetailModel model)
        {
            List<TransactionReturnModel> transactionReturnModels = new List<TransactionReturnModel>();
            SecurityToken token = await GetSecurityToken();
            var client = new PaymentGateway.IeBizServiceClient();
            var data = await client.SearchTransactionsAsync(token, model.searchFilters, model.matchAll, model.countOnly, model.start, model.limit, model.sort);
            string val = data.TransactionsReturned;
            int length = Convert.ToInt32(val);
            TransactionReturnModel transactionReturn = new TransactionReturnModel();
            TransactionObject[] trans = new TransactionObject[length];
            transactionReturn.transactionObjects = data.Transactions.ToList();
            transactionReturnModels.Add(transactionReturn);
            return transactionReturnModels;
        }
        //Default PaymentMethod Profile
        public async Task<dynamic> SetDefaultPaymentMethodProfile(EbizPaymentProfile model)
        {
            Response response = new Response();
            SecurityToken token = await GetSecurityToken();
            var client = new PaymentGateway.IeBizServiceClient();
            var result = await client.SetDefaultCustomerPaymentMethodProfileAsync(token, model.customerToken, model.paymentMethod);
            return result;
        }
        //Update Customer Payment Method Profile
        public async Task<dynamic> UpdatePaymentMethodProfile(EbizUpdatePaymentProfile model)
        {
            Response response = new Response();
            SecurityToken token = await GetSecurityToken();
            var client = new PaymentGateway.IeBizServiceClient();
            model.customerToken = "11166583";
            string CCnum = "4000100011112224";
            PaymentMethodProfile obj = new PaymentMethodProfile();
            obj.MethodID = "1205";
            //obj.CardNumber = "XXXXXX" + CCnum.Substring(6, (CCnum.Length - 6));
            obj.CardNumber = "4000100011112224";
            obj.CardExpiration = "1225";
            obj.AvsStreet = "20 Pacifica";
            obj.AvsZip = "92618";
            obj.MethodName = "New Name";
            obj.AccountHolderName = "Tim Smith";
            model.paymentMethod = obj;
            var result = await client.UpdateCustomerPaymentMethodProfileAsync(token, model.customerToken, model.paymentMethod);
            return result;
        }
        public async Task<dynamic> AddNewPaymentDetails(string id, PaymentMethodProfile payMethod)
        {
            Response response = new Response();
            SecurityToken token = await GetSecurityToken();
            var client = new PaymentGateway.IeBizServiceClient();
            response.PaymentMethodProfileResponse = await client.AddCustomerPaymentMethodProfileAsync(token, id, payMethod);
            return response.PaymentMethodProfileResponse;
        }
        //Add New Customer
        public async Task<dynamic> AddNewCustomer(Customer customer)
        {
            Response response = new Response();
            SecurityToken token = await GetSecurityToken();
            var client = new PaymentGateway.IeBizServiceClient();
            response.CustomerResponse = await client.AddCustomerAsync(token, customer);
            return response.CustomerResponse;
        }
        //Get Customer
        public async Task<dynamic> GetCustomerDetails(CustomerDetailModel model)
        {
            SecurityToken token = await GetSecurityToken();
            var client = new PaymentGateway.IeBizServiceClient();
            var data = await client.GetCustomerAsync(token, model.customerId, model.customerInternalId);
            return data;
        }
        //Search All Customer
        public async Task<dynamic> SearchCustomerDetails(CustomerDetailModel model)
        {
            SecurityToken token = await GetSecurityToken();
            var client = new PaymentGateway.IeBizServiceClient();
            List<CustomerReturnModel> customerReturnModels = new List<CustomerReturnModel>();
            int length = 0;
            int lengthnode = 0;
            do
            {
                lengthnode++;
                var data = await client.SearchCustomerListAsync(token,model.searchFilters,model.start, model.limit, model.sort, model.includeCustomerToken, model.includePaymentMethodProfile,model.countOnly);
                length = data.Count;
                CustomerReturnModel customerReturn = new CustomerReturnModel();
                customerReturn.customers = data.CustomerList.ToList();
                customerReturnModels.Add(customerReturn);
                model.start = Int32.Parse(lengthnode + "000");
                model.limit = Int32.Parse(lengthnode + "999");
            } while (length >= 1000);
            return customerReturnModels;
        }
        //Search Customer with pagination
        public async Task<dynamic> SearchCustomersWithPagination(CustomerDetailModel model)
        {
            SecurityToken token = await GetSecurityToken();
            var client = new PaymentGateway.IeBizServiceClient();
            List<CustomerReturnModel> customerReturnModels = new List<CustomerReturnModel>();
            int length = 0;
            int lengthnode = 0;
            do
            {
                lengthnode++;
                var data = await client.SearchCustomerListAsync(token, model.searchFilters, model.start, model.limit, model.sort, model.includeCustomerToken, model.includePaymentMethodProfile, model.countOnly);
                length = data.Count;
                CustomerReturnModel customerReturn = new CustomerReturnModel();
                customerReturn.customers = data.CustomerList.ToList();
                customerReturnModels.Add(customerReturn);
                model.start = Int32.Parse(lengthnode + "000");
                model.limit = Int32.Parse(lengthnode + "999");
            } while (length >= 1000);
            return customerReturnModels;
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
            var data = await client.SearchSalesOrdersAsync(token, model.customerId, model.subCustomerId, model.salesOrderNo, model.salesOrderInternalId,model.searchFilters, model.start,model.limit,model.sort,model.includeItems);
            return data;
        }
        //Search All Orders with Pagination
        public async Task<dynamic> SearchOrdersWithPagination(OrderDetailModel model)
        {
            SecurityToken token = await GetSecurityToken();
            var client = new PaymentGateway.IeBizServiceClient();
            var data = await client.SearchSalesOrdersAsync(token, model.customerId, model.subCustomerId, model.salesOrderNo, model.salesOrderInternalId, model.searchFilters, model.start, model.limit, model.sort, model.includeItems);
            return data;
        }
    }
}
