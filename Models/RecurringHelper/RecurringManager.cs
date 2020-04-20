using PaymentGateway;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models.RecurringHelper
{
    public class RecurringManager
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
        public async Task<dynamic> AddRecurringPayment(RecurringDetailModel model)
        {
            SecurityToken token = await GetSecurityToken();
            var client = new PaymentGateway.IeBizServiceClient();
            //Customer customer = await client.GetCustomerAsync(token,"409","");
            CustomSchedule obj = new CustomSchedule();
            obj.Interval = 10;
            obj.Frequency = "10";
            model.recurringBilling.CustomScheduleObject = obj;
            System.Globalization.CultureInfo customCulture = new System.Globalization.CultureInfo("en-US", true);
            customCulture.DateTimeFormat.ShortDatePattern = "yyyy-MM-dd";
            System.Threading.Thread.CurrentThread.CurrentCulture = customCulture;
            System.Threading.Thread.CurrentThread.CurrentUICulture = customCulture;
            model.startDate = System.Convert.ToDateTime(model.startDate.ToString("yyyy-MM-dd"));
            model.nextDate = System.Convert.ToDateTime(model.nextDate.ToString("yyyy-MM-dd"));
            model.endDate = System.Convert.ToDateTime(model.endDate.ToString("yyyy-MM-dd"));
            string dt = model.endDate.ToShortDateString();
            DateTime ss = Convert.ToDateTime(dt);
            RecurringBilling recurringBilling = new RecurringBilling
            {
                Amount = 100,
                Schedule = "monthly",
                ScheduleName = "Monthly billing",
                SendCustomerReceipt = false,
                Start = model.startDate,
                Next = model.nextDate,
                Expire = model.endDate,
                CustomScheduleObject = new CustomSchedule { Frequency = "1", Interval = 5 },
                Enabled = true,
                Tax = 20,
                ReceiptTemplateName = "",
                ReceiptNote = "okay",
                RepeatCount = 20,
                NumLeft = "",
            };
            model.recurringBilling = recurringBilling;
             dynamic response = await client.ScheduleRecurringPaymentAsync(token, model.customerInternalId,model.paymentMethodProfileId,model.recurringBilling);
            dynamic response1 = await client.ModifyScheduledRecurringPayment_PaymentMethodProfileAsync(token,model.scheduledPaymentInternalId, model.paymentMethodProfileId);
            dynamic response2 = await client.MarkRecurringPaymentAsAppliedAsync(token,model.invoiceNumber, model.paymentInternalId);
            return response;
        }
        public async Task<dynamic> ModifyScheduledRecurringPaymentStatus(RecurringDetailModel model)
        {
            SecurityToken token = await GetSecurityToken();
            var client = new PaymentGateway.IeBizServiceClient();
            dynamic response = await client.ModifyScheduledRecurringPaymentStatusAsync(token, model.scheduledPaymentInternalId,model.statusId);
            return response;
        }
        
        public async Task<dynamic> DeleteRecurringPayments(RecurringDetailModel model)
        {
            SecurityToken token = await GetSecurityToken();
            var client = new PaymentGateway.IeBizServiceClient();
            Customer customer = await client.GetCustomerAsync(token, "409", "");
            dynamic response = await client.ModifyScheduledRecurringPaymentStatusAsync(token, model.scheduledPaymentInternalId, model.statusId);
            return response;
        }
        public async Task<dynamic> SearchRecurringPayments(RecurringDetailModel model)
        {
            SecurityToken token = await GetSecurityToken();
            var client = new PaymentGateway.IeBizServiceClient();
            Customer customer = await client.GetCustomerAsync(token,"409","");
            dynamic response = await client.SearchRecurringPaymentsAsync(token, model.scheduledPaymentInternalId, model.customerId,model.customerInternalId,model.fromDateTime,model.toDateTime,model.start,model.limit,model.sort);
            return response;
        }
        public async Task<dynamic> SearchScheduledRecurringPayments(RecurringDetailModel model)
        {
            //ok
            SecurityToken token = await GetSecurityToken();
            var client = new PaymentGateway.IeBizServiceClient();
            dynamic response = await client.SearchScheduledRecurringPaymentsAsync(token,model.customerInternalId, model.customerId, model.start, model.limit, model.sort);
            return response;
        }
        public async Task<dynamic> SearchReceivedRecurringPayments(RecurringDetailModel model)
        {
            SecurityToken token = await GetSecurityToken();
            var client = new PaymentGateway.IeBizServiceClient();
            dynamic response = await client.ScheduleRecurringPaymentAsync(token, model.customerInternalId, model.paymentMethodProfileId, model.recurringBilling);
            return response;
        }
    }
}
