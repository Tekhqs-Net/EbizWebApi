using PaymentGateway;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Models.CustomerHelper;
using WebApi.Models.DynamicResponse;
using WebApi.Models.EbizModelHelper;

namespace WebApi.Models.EmailHelper
{
    public class EmailManager
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
        //Get Email Template
        public async Task<dynamic> GetEmailTemplate(EmailDetailModel model)
        {
            EmailResponse response = new EmailResponse(); 
            SecurityToken token = await GetSecurityToken();
            var client = new PaymentGateway.IeBizServiceClient();
            var template = await client.GetEmailTemplatesAsync(token,"","");
            var temp = template.Where(x => x.TemplateTypeId == model.ebizWebForm.EmailTemplateID).FirstOrDefault();
            if (temp?.TemplateTypeId == "WebFormEmail")
            {
                model.ebizWebForm.FromEmail = temp.FromEmail;
                model.ebizWebForm.FromName = temp.FromName;
                response = await GetEmailPaymentTemplate(model);
            }
            else if (temp?.TemplateTypeId == "AddPaymentMethodFormEmail")
            {
                model.ebizWebForm.FromEmail = temp.FromEmail;
                model.ebizWebForm.FromName = temp.FromName;
                response = await AddPaymentMethodTemplate(model);
            }
            return response;
           
        }

        public async Task<EmailResponse> GetEmailPaymentTemplate(EmailDetailModel model)
        {
            EmailResponse response = new EmailResponse();
            SecurityToken token = await GetSecurityToken();
            var client = new PaymentGateway.IeBizServiceClient();
            if (!string.IsNullOrEmpty(model.ebizWebForm?.CustomerId))
            {
                try
                {
                    Customer customer = await client.GetCustomerAsync(token, model.ebizWebForm.CustomerId, "");
                    model.ebizWebForm.CustFullName = customer.FirstName + " " + customer.LastName;
                    model.customer = customer;
                }
                catch (Exception ex)
                {
                    if (ex.Message.ToString() == "Not Found" && model.customer != null)
                    {
                        Customer customer = new Customer();
                        customer.CustomerId = model.ebizWebForm.CustomerId;
                        customer.CompanyName = model.ebizWebForm.CustFullName;
                        //model.customer
                        response.Customer = await new CustomerManager().AddNewCustomer(customer, token);
                        model.ebizWebForm.CustomerId = response.Customer.CustomerId;
                        model.ebizWebForm.CustFullName = model.customer.FirstName + " " + model.customer.LastName;
                        model.customer.CustomerInternalId = response.Customer.CustomerInternalId;
                    }
                }
            }
            //ShowViewSalesOrderLink
            //ShowViewInvoiceLink
            if (!string.IsNullOrEmpty(model.ebizWebForm?.InvoiceNumber))
            {
                try
                {
                    var invoice = await client.GetInvoiceAsync(token, model.ebizWebForm.CustomerId, "", model.ebizWebForm.InvoiceNumber, model.ebizWebForm.InvoiceInternalId);
                    model.ebizWebForm.InvoiceNumber = invoice.InvoiceNumber;
                    model.ebizWebForm.InvoiceInternalId = invoice.InvoiceInternalId;
                }
                catch (Exception ex)
                {
                    if (ex.Message.ToString() == "Not Found" && model.invoice != null)
                    {
                        //model.invoice
                        Invoice invoice = new Invoice();
                        invoice.CustomerId = model.ebizWebForm.CustomerId;
                        invoice.AmountDue = model.ebizWebForm.AmountDue;
                        invoice.InvoiceNumber = model.ebizWebForm?.InvoiceNumber;
                        invoice.InvoiceDueDate = model.ebizWebForm.DueDate?.ToString();
                        var addinvoice = await client.AddInvoiceAsync(token, invoice);
                        response.Invoice = addinvoice;
                        model.ebizWebForm.InvoiceNumber = invoice.InvoiceNumber;
                        model.ebizWebForm.InvoiceInternalId = addinvoice.InvoiceInternalId;
                    }
                }
            }
            if (!string.IsNullOrEmpty(model.ebizWebForm?.OrderId))
            {
                try
                {
                    var salesOrder = await client.GetSalesOrderAsync(token, model.ebizWebForm?.CustomerId, model.customer?.CustomerInternalId, model.ebizWebForm?.OrderId, model.salesOrder?.SalesOrderInternalId);
                    model.ebizWebForm.SalesOrderInternalId = salesOrder.SalesOrderInternalId;
                }
                catch (Exception ex)
                {
                    if (ex.Message.ToString() == "Not Found" && model.salesOrder != null)
                    {
                        //model.salesOrder
                        //SalesOrder salesOrder = new SalesOrder();
                        var addOrder = await client.AddSalesOrderAsync(token, model.salesOrder);
                        response.SalesOrder = addOrder;
                        model.ebizWebForm.SalesOrderInternalId = addOrder.SalesOrderInternalId;
                    }
                }
            }
            var data = await client.GetEbizWebFormURLAsync(token, model.ebizWebForm);
            response.EbizWebFormLink = data;
            return response;
        }
        public async Task<EmailResponse> AddPaymentMethodTemplate(EmailDetailModel model)
        {
            SecurityToken token = await GetSecurityToken();
            var client = new PaymentGateway.IeBizServiceClient();
            EmailResponse response = new EmailResponse();
            if (!string.IsNullOrEmpty(model.ebizWebForm?.CustomerId))
            {
                try
                {
                    Customer customer = await client.GetCustomerAsync(token, model.ebizWebForm.CustomerId, "");
                    model.ebizWebForm.CustFullName = customer.FirstName + " " + customer.LastName;
                    model.customer = customer;
                }
                catch (Exception ex)
                {
                    if (ex.Message.ToString() == "Not Found" && model.customer != null)
                    {
                        Customer customer = new Customer();
                        customer.CustomerId = model.ebizWebForm.CustomerId;
                        customer.CompanyName = model.ebizWebForm.CustFullName;
                        //model.customer
                        response.Customer = await new CustomerManager().AddNewCustomer(customer, token);
                        model.ebizWebForm.CustomerId = response.Customer.CustomerId;
                        model.ebizWebForm.CustFullName = model.customer.FirstName + " " + model.customer.LastName;
                        model.customer.CustomerInternalId = response.Customer.CustomerInternalId;
                    }
                }
            }
            model.ebizWebForm.EmailAddress = "abutt@tekhqs.com";
            model.ebizWebForm.SendEmailToCustomer = true;
            var data = await client.GetEbizWebFormURLAsync(token, model.ebizWebForm);
            response.EbizWebFormLink = data;
            return response;
        }

        //Get Email Template
        public async Task<dynamic> SearchEbizWebFormPaymentRcvd(EmailDetailModel model)
        {
            SecurityToken token = await GetSecurityToken();
            var client = new PaymentGateway.IeBizServiceClient();
            TimeSpan ts = new TimeSpan(00, 00, 0);
            model.fromPaymentRequestDateTime = model.fromPaymentRequestDateTime.Date + ts;
            TimeSpan ts2 = new TimeSpan(23, 59, 0);
            model.toPaymentRequestDateTime = model.toPaymentRequestDateTime.Date + ts2;
            dynamic response = await client.SearchEbizWebFormReceivedPaymentsAsync(token,model.customerId,model.fromPaymentRequestDateTime,model.toPaymentRequestDateTime,model.searchFilters, model.start,model.limit,model.sort);
            return response;
        }
        public async Task<dynamic> MarkEbizWebFormPaymentAsApplied(EmailDetailModel model)
        {
            SecurityToken token = await GetSecurityToken();
            var client = new PaymentGateway.IeBizServiceClient();
            dynamic response = await client.MarkEbizWebFormPaymentAsAppliedAsync(token, model.paymentInternalId);
            return response;
        }
        public async Task<dynamic> DeleteEBizWebFormPayment(EmailDetailModel model)
        {
            SecurityToken token = await GetSecurityToken();
            var client = new PaymentGateway.IeBizServiceClient();
            dynamic response = await client.DeleteEbizWebFormPaymentAsync(token, model.paymentInternalId);
            return response;
        }
        public async Task<dynamic> SearchEbizWebFormPendingPayments(EmailDetailModel model)
        {
            SecurityToken token = await GetSecurityToken();
            var client = new PaymentGateway.IeBizServiceClient();
            TimeSpan ts = new TimeSpan(00, 00, 0);
            model.fromPaymentRequestDateTime = model.fromPaymentRequestDateTime.Date + ts;
            TimeSpan ts2 = new TimeSpan(23, 59, 0);
            model.toPaymentRequestDateTime = model.toPaymentRequestDateTime.Date + ts2;
            dynamic response = await client.SearchPaymentFormPendingPaymentsAsync(token, model.customerId, model.fromPaymentRequestDateTime, model.toPaymentRequestDateTime, model.start, model.limit, model.sort);
            return response;
        }
        public async Task<dynamic> ResendEbizWebFormEmail(EmailDetailModel model)
        {
            //response issue
            SecurityToken token = await GetSecurityToken();
            var client = new PaymentGateway.IeBizServiceClient();
            dynamic response = await client.ResendEbizWebFormEmailAsync(token, model.paymentInternalId);
            return response;
        }

    }
}
