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
            SecurityToken token = await GetSecurityToken();
            var client = new PaymentGateway.IeBizServiceClient();
            if (model.customer.CustomerToken == "" || model.customer.CustomerToken == null)
            {
                dynamic customer = await client.GetCustomerAsync(token, model.customer.CustomerId, "");
                model.customer.CustomerToken = customer.CustomerToken;
            }
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

        //Default PaymentMethod Profile
        public async Task<dynamic> DefaultPaymentMethodProfile(EbizPaymentProfile model)
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
    }
}
