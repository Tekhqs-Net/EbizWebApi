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
                card.CardNumber = "4000100011112224";
                card.CardExpiration = "0922";
                card.CardCode = "123";
                card.AvsStreet = "123 Main st.";
                card.AvsZip = "90046";
                model.transactionRequest.CreditCardData = card;
                model.transactionRequest.CustReceipt = true;
                model.transactionRequest.CustReceiptName = "Aaadam Smith";
                //model.transactionRequest.Command = "AuthOnly";
                model.transactionRequest.Software = "WebApiApp";
                TransactionDetail details = new TransactionDetail();
                details.Amount = 2;
                details.Description = "Example QuickSale";
                details.Invoice = "1286";
                model.transactionRequest.Details = details;
                string caseSwitch = model.transactionRequest.Command;
                switch (caseSwitch)
                {
                    case "Sale":
                        // cared details pass
                        break;
                    case "AuthOnly":
                        // cared details pass
                        break;
                    case "Void":
                        model.transactionRequest.RefNum = "";
                        model.transactionRequest.CustomerID = "";
                        break;
                    case "Capture":
                        model.transactionRequest.RefNum = "";
                        model.transactionRequest.CustomerID = "";
                        break;
                    case "Credit":
                        model.transactionRequest.RefNum = "";
                        model.transactionRequest.CustomerID = "";
                        break;
                    case "CreditVoid":
                        break;
                    case "PostAuth":
                        break;
                    case "Check":
                        //check details in paymentmethodprofile
                        break;
                    case "CheckCredit":
                        break;
                    default:
                        break;
                }

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
                model.customerTransaction.Software = model.customer?.SoftwareId;
                PaymentMethodProfile payMethod = new PaymentMethodProfile();
                payMethod.CardExpiration = "0922";
                payMethod.CardNumber = "4000100511112229";
                payMethod.AvsStreet = "123 Main st.";
                payMethod.AvsZip = "90046";
                payMethod.CardCode = "999";
                payMethod.MethodName = "My Visa";
                model.paymentMethodProfile = payMethod;
                response = await new ServiceHandler().SaveCustTransaction(model);
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
                var response = await new ServiceHandler().SetDefaultPaymentMethodProfile(model);
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
        //Search Transaction
        [Route("[action]")]
        [HttpPost]
        public async Task<IActionResult> GetTransaction([FromBody]GetTransactionDetailModel model)
        {
            try
            {
                dynamic response = await new ServiceHandler().GetTransactionDetails(model);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        //Search All Transaction
        [Route("[action]")]
        [HttpPost]
        public async Task<IActionResult> SearchAllTransaction([FromBody]SearchTransactionDetailModel model)
        {
            try
            {
                List<TransactionReturnModel> response = await new ServiceHandler().SearchAllTransactionDetails(model);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        //Search  Transaction with pagination
        [Route("[action]")]
        [HttpPost]
        public async Task<IActionResult> SearchTransactionwithPagination([FromBody]SearchTransactionDetailModel model)
        {
            try
            {
                List<TransactionReturnModel> response = await new ServiceHandler().SearchTransactionWithPagination(model);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        // Get Customer
        [Route("[action]")]
        [HttpPost]
        public async Task<IActionResult> GetCustomer([FromBody]CustomerDetailModel model)
        {
            try
            {
                dynamic response = await new ServiceHandler().GetCustomerDetails(model);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Route("[action]")]
        [HttpPost]
        public async Task<IActionResult> SearchAllCustomers([FromBody]CustomerDetailModel model)
        {
            try
            {
                dynamic response = await new ServiceHandler().SearchCustomerDetails(model);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Route("[action]")]
        [HttpPost]
        public async Task<IActionResult> SearchCustomersWithPagination([FromBody]OrderDetailModel model)
        {
            try
            {
                dynamic response = await new ServiceHandler().SearchCustomersWithPagination(model);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        //under progress
        //Get  Transaction 
        [Route("[action]")]
        [HttpPost]
        public async Task<IActionResult> GetOrder([FromBody]OrderDetailModel model)
        {
            try
            {
                dynamic response = await new ServiceHandler().GetOrderDetails(model);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Route("[action]")]
        [HttpPost]
        public async Task<IActionResult> SearchAllOrders([FromBody]OrderDetailModel model)
        {
            try
            {
                dynamic response = await new ServiceHandler().SearchAllOrders(model);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Route("[action]")]
        [HttpPost]
        public async Task<IActionResult> SearchOrdersWithPagination([FromBody]OrderDetailModel model)
        {
            try
            {
                dynamic response = await new ServiceHandler().SearchOrdersWithPagination(model);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Route("[action]")]
        [HttpPost]
        public async Task<IActionResult> GetItem([FromBody]OrderDetailModel model)
        {
            try
            {
                dynamic response = await new ServiceHandler().GetOrderDetails(model);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Route("[action]")]
        [HttpPost]
        public async Task<IActionResult> SearchAllItems([FromBody]OrderDetailModel model)
        {
            try
            {
                dynamic response = await new ServiceHandler().SearchAllOrders(model);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Route("[action]")]
        [HttpPost]
        public async Task<IActionResult> SearchItemsWithPagination([FromBody]OrderDetailModel model)
        {
            try
            {
                dynamic response = await new ServiceHandler().SearchOrdersWithPagination(model);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Route("[action]")]
        [HttpPost]
        public async Task<IActionResult> GetInvoice([FromBody]OrderDetailModel model)
        {
            try
            {
                dynamic response = await new ServiceHandler().GetOrderDetails(model);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Route("[action]")]
        [HttpPost]
        public async Task<IActionResult> SearchAllInvoices([FromBody]OrderDetailModel model)
        {
            try
            {
                dynamic response = await new ServiceHandler().SearchAllOrders(model);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Route("[action]")]
        [HttpPost]
        public async Task<IActionResult> SearchInvoicesWithPagination([FromBody]OrderDetailModel model)
        {
            try
            {
                dynamic response = await new ServiceHandler().SearchOrdersWithPagination(model);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
