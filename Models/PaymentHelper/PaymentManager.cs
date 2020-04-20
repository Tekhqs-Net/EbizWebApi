using PaymentGateway;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Models.DynamicResponse;
using WebApi.Models.EbizModelHelper;
using WebApi.Models.TransactionHelper;

namespace WebApi.Models.PaymentHelper
{
    public class PaymentManager
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
        //ReceivedPayment
        public async Task<dynamic> ReceivedPayment(PaymentDetailModel model)
        {
            Response response = new Response();
            SecurityToken token = await GetSecurityToken();
            var client = new PaymentGateway.IeBizServiceClient();
            var result = await client.GetPaymentsAsync(token, model.customerId,model.customerInternalId,model.fromDateTime, model.toDateTime, model.start, model.limit, model.sort);
            return result;
        }
        //MarkPaymentAsApplied
        public async Task<dynamic> MarkPaymentAsApplied(PaymentDetailModel model)
        {
            Response response = new Response();
            SecurityToken token = await GetSecurityToken();
            var client = new PaymentGateway.IeBizServiceClient();
            var result = await client.MarkPaymentAsAppliedAsync(token, model.invoiceNumber, model.paymentInternalId);
            return result;
        }
        //Delete PaymentMethod Profile
        public async Task<dynamic> DeletePaymentMethodProfile(PaymentDetailModel model)
        {
            Response response = new Response();
            SecurityToken token = await GetSecurityToken();
            var client = new PaymentGateway.IeBizServiceClient();
            var result = await client.DeleteCustomerPaymentMethodProfileAsync(token, model.customerToken, model.paymentMethod);
            return result;
        }
        //Default PaymentMethod Profile
        public async Task<dynamic> SetDefaultPaymentMethodProfile(PaymentProfile model)
        {
            Response response = new Response();
            SecurityToken token = await GetSecurityToken();
            var client = new PaymentGateway.IeBizServiceClient();
            var result = await client.SetDefaultCustomerPaymentMethodProfileAsync(token, model.customerToken, model.paymentMethod);
            return result;
        }
        //Update Customer Payment Method Profile
        public async Task<dynamic> UpdatePaymentMethodProfile(PaymentDetailModel model)
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
            model.paymentMethodProfile = obj;
            var result = await client.UpdateCustomerPaymentMethodProfileAsync(token, model.customerToken, model.paymentMethodProfile);
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
    }
}
