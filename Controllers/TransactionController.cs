﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PaymentGateway;
using WebApi.Models.DynamicResponse;
using WebApi.Models.EbizModelHelper;
using WebApi.Models.TransactionHelper;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        //Search Items with pagination
        [Route("[action]")]
        [HttpPost]
        public async Task<IActionResult> AVSVerifyRelease([FromBody]RunTransactionModel model)
        {
            try
            {
                //CreditCardData card = new CreditCardData();
                //card.CardNumber = "4000100011112224";
                //card.CardExpiration = "0922";
                //card.CardCode = "123";
                //card.AvsStreet = "123 Main st.";
                //card.AvsZip = "90046";
                //model.transactionRequest.CreditCardData = card;
                //model.transactionRequest.CustReceipt = true;
                //model.transactionRequest.CustReceiptName = "Aaadam Smith";
                model.transactionRequest.Command = "AuthOnly";
                model.transactionRequest.Software = "WebApiApp";
                model.transactionRequest.CustomerID = "409";
                TransactionDetail details = new TransactionDetail();
                details.Amount = 0.5;
                //details.Description = "Example QuickSale";
                //details.Invoice = "1286";
                model.transactionRequest.Details = details;
                dynamic response = await new TransactionManager().AVSPaymentResponse(model);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //API for transaction with existing card
        [Route("[action]")]
        [HttpPost]
        public async Task<IActionResult> RunTransaction([FromBody]RunTransactionModel model)
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
                model.transactionRequest.Command = "AuthOnly";
                model.transactionRequest.Software = "WebApiApp";
                TransactionDetail details = new TransactionDetail();
                details.Amount = 2;
                details.Description = "Example QuickSale";
                details.Invoice = "1286";
                model.transactionRequest.Details = details;
                string caseSwitch = model.transactionRequest.Command;
                response.TransResponse = await new TransactionManager().NewTransaction(model);
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
        public async Task<IActionResult> RunCustTransaction(CustomerTransactionModel model)
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
                response = await new TransactionManager().SaveCustTransaction(model);
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
        public async Task<IActionResult> GetTransaction([FromBody]TransactionDetailModel model)
        {
            try
            {
                dynamic response = await new TransactionManager().GetTransactionDetails(model);
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
        public async Task<IActionResult> SearchAllTransaction([FromBody]TransactionDetailModel model)
        {
            try
            {
                List<TransactionReturnModel> response = await new TransactionManager().SearchAllTransactionDetails(model);
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
        public async Task<IActionResult> SearchTransactionwithPagination([FromBody]TransactionDetailModel model)
        {
            try
            {
                var response = await new TransactionManager().SearchTransactionWithPagination(model);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}