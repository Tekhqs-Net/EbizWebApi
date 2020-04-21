using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApi.Models.EbizModelHelper;
using WebApi.Models.EmailHelper;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        [Route("[action]")]
        [HttpPost]
        public async Task<IActionResult> GetEmailTemplate([FromBody]EmailDetailModel model)
        {
            try
            {
                if (string.IsNullOrEmpty(model.ebizWebForm?.EmailTemplateID))
                {
                    string msg = "EmailTemplateID should not be empty or null.";
                    return BadRequest(msg);
                }
                // model.ebizWebForm.CustomerId = "409";
                model.ebizWebForm.ProcessingCommand = "Sale";
                //model.ebizWebForm.EmailTemplateID = "c5eabda5-26ca-4724-b361-25f542714b3e";
                //model.ebizWebForm.EmailAddress = "";
                dynamic response = await new EmailManager().GetEmailTemplate(model);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Route("[action]")]
        [HttpPost]
        public async Task<IActionResult> SearchEbizWebFormPaymentRcvd([FromBody]EmailDetailModel model)
        {
            try
            {
              
                dynamic response = await new EmailManager().SearchEbizWebFormPaymentRcvd(model);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Route("[action]")]
        [HttpPost]
        public async Task<IActionResult> MarkEbizWebFormPaymentAsApplied([FromBody]EmailDetailModel model)
        {
            try
            {
                if (string.IsNullOrEmpty(model.paymentInternalId))
                {
                    string msg = "paymentInternalId should not be null or empty";
                    return BadRequest(msg);
                }
                dynamic response = await new EmailManager().MarkEbizWebFormPaymentAsApplied(model);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Route("[action]")]
        [HttpPost]
        public async Task<IActionResult> DeleteEBizWebFormPayment([FromBody]EmailDetailModel model)
        {
            try
            {
                if (string.IsNullOrEmpty(model.paymentInternalId))
                {
                    string msg = "paymentInternalId should not be null or empty";
                    return BadRequest(msg);
                }
                dynamic response = await new EmailManager().DeleteEBizWebFormPayment(model);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Route("[action]")]
        [HttpPost]
        public async Task<IActionResult> SearchEbizWebFormPendingPayments([FromBody]EmailDetailModel model)
        {
            try
            {

                dynamic response = await new EmailManager().SearchEbizWebFormPendingPayments(model);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Route("[action]")]
        [HttpPost]
        public async Task<IActionResult> ResendEbizWebFormEmail([FromBody]EmailDetailModel model)
        {
            try
            {
                if (string.IsNullOrEmpty(model.paymentInternalId))
                {
                    string msg = "paymentInternalId should not be null or empty";
                    return BadRequest(msg);
                }
                dynamic response = await new EmailManager().ResendEbizWebFormEmail(model);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}