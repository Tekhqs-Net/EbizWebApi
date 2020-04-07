using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using PaymentGateway;
using WebApi.Models.EbizModelHelper;
using WebApi.Models.ServiceHelper;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EBizChargeController : ControllerBase
    {
        //API for transaction with existing card
        [Route("[action]")]
        [HttpPost]
        public async Task<IActionResult> RunTransaction([FromBody]EbizRunTranModel model)
        {
            try
            {
                Response response = new Response();
                CreditCardData card = new CreditCardData();
                //card.CardNumber = customer.PaymentMethodProfiles[0].CardNumber;
                //card.CardExpiration = customer.PaymentMethodProfiles[0].CardExpiration;
                //card.CardCode= customer.PaymentMethodProfiles[0].CardCode;
                //card.AvsZip = customer.PaymentMethodProfiles[0].AvsZip;
                //card.AvsStreet = customer.PaymentMethodProfiles[0].AvsStreet;
                card.CardNumber = "4000100011112224";
                card.CardExpiration = "0922";
                card.CardCode = "123";
                card.AvsStreet = "123 Main st.";
                card.AvsZip = "90046";
                model.transactionRequest.CreditCardData = card;
                //model.transactionRequest.CustomerID = customer.CustomerId;
                //model.transactionRequest.AccountHolder = customer.CompanyName;
                model.transactionRequest.CustReceipt = true;
                model.transactionRequest.CustReceiptName = "Aaadam Smith";
                model.transactionRequest.Command = "AuthOnly";
                model.transactionRequest.Software = "WebApiApp";
                TransactionDetail details = new TransactionDetail();
                details.Amount = 2;
                details.Description = "Example QuickSale";
                details.Invoice = "1286";
                model.transactionRequest.Details = details;
                response.TransResponse = await new ServiceHandler().NewTransaction(model);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Route("[action]")]
        //API for transaction with new card without saving
        [HttpPost]
        public async Task<IActionResult> RunCustTransaction(EbizCustTranModel model)
        {
            try
            {
                Response response = new Response();
                TransactionDetail details = new TransactionDetail();
                details.Amount = 2;
                details.Description = "Example QuickSale";
                details.Invoice = "1286";
                model.customerTransaction.Details = details;
                model.customerTransaction.Software = ".NetApi";
                model.customerTransaction.CustReceipt = true;
                model.customerTransaction.CustReceiptName = "Aaadam Smith";
                model.customerTransaction.Command = "AuthOnly";
                model.customerTransaction.Software = model.customer.SoftwareId;
                if (model.custToken == "")
                {
                    //custtoken is not passed its mean cstmr not exist
                    PaymentMethodProfile payMethod = new PaymentMethodProfile();
                    payMethod.CardExpiration = "0922";
                    payMethod.CardNumber = "4000100511112229";
                    payMethod.AvsStreet = "123 Main st.";
                    payMethod.AvsZip = "90046";
                    payMethod.CardCode = "999";
                    payMethod.MethodName = "My Visa";
                    response.CustomerResponse = await new ServiceHandler().AddNewCustomer(model.customer);
                    if (response.CustomerResponse.CustomerInternalId != "")
                    {
                        response.PaymentMethodProfileResponse = await new ServiceHandler().AddNewPaymentDetails(response.CustomerResponse.CustomerInternalId, payMethod);
                    }
                    model.customer.CustomerId = response.CustomerResponse.CustomerId;
                    model.paymentMethod = response.PaymentMethodProfileResponse;
                    response.TransResponse = await new ServiceHandler().SaveCustTransaction(model);
                }
                else if (model.custToken != "" && model.paymentMethod == null)
                {
                    //in this customer exist but paymentmethod is not passed so we save new paymentdetails
                    PaymentMethodProfile payMethod = new PaymentMethodProfile();
                    payMethod.CardExpiration = "0922";
                    payMethod.CardNumber = "4000100511112229";
                    payMethod.AvsStreet = "123 Main st.";
                    payMethod.AvsZip = "90046";
                    payMethod.CardCode = "999";
                    payMethod.MethodName = "My Visa";
                    response.PaymentMethodProfileResponse = await new ServiceHandler().AddNewPaymentDetails(model.customer.CustomerInternalId, payMethod);
                    response.TransResponse = await new ServiceHandler().SaveCustTransaction(model);
                }
                else
                {
                    response.TransResponse = await new ServiceHandler().SaveCustTransaction(model);
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //API for transaction with new card and save that card info against customer on gateway
        //[Route("[action]")]
        //public ActionResult RunCustTransactionWithSavingCard(EbizDetailModel model)
        //{
        //    try
        //    {
        //        string json = "Success";
        //        return Ok(json);
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}
    //Delete Payment Method Profile
    [Route("[action]")]
    [HttpPost]
    public async Task<IActionResult> DeletePaymentMethodProfile([FromBody]EbizPaymentProfile model)
    {
        try
        {
            var response = await new ServiceHandler().DeletePaymentMethodProfile(model);
            return Ok(response);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
       
        // default payment method profile
        [Route("[action]")]
        [HttpPost]
        public async Task<IActionResult> DefaultPaymentMethodProfile([FromBody]EbizPaymentProfile model)
        {
            try
            {
                var response = await new ServiceHandler().DefaultPaymentMethodProfile(model);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        //Update Payment Method Profile
        [Route("[action]")]
        [HttpPost]
        public async Task<IActionResult> UpdatePaymentMethodProfile([FromBody]EbizUpdatePaymentProfile model)
        {
            try
            {
                Response response = new Response();
                response.TransResponse = await new ServiceHandler().UpdatePaymentMethodProfile(model);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
