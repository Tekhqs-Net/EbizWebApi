using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApi.Models.DynamicResponse;
using WebApi.Models.EbizModelHelper;
using WebApi.Models.PaymentHelper;
using WebApi.Models.TransactionHelper;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        //Received Payment 
        [Route("[action]")]
        [HttpPost]
        public async Task<IActionResult> ReceivedPayment([FromBody]PaymentDetailModel model)
        {
            try
            {
                Response response = new Response();
                response.TransResponse = await new PaymentManager().ReceivedPayment(model);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        //MarkPaymentAsApplied
        [Route("[action]")]
        [HttpPost]
        public async Task<IActionResult> MarkPaymentAsApplied([FromBody]PaymentDetailModel model)
        {
            try
            {
                Response response = new Response();
                response.TransResponse = await new PaymentManager().MarkPaymentAsApplied(model);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        //Delete Payment Method Profile
        [Route("[action]")]
        [HttpPost]
        public async Task<IActionResult> DeletePaymentMethodProfile([FromBody]PaymentDetailModel model)
        {
            try
            {
                var response = await new PaymentManager().DeletePaymentMethodProfile(model);
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
        public async Task<IActionResult> DefaultPaymentMethodProfile([FromBody]PaymentProfile model)
        {
            try
            {
                var response = await new PaymentManager().SetDefaultPaymentMethodProfile(model);
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
        public async Task<IActionResult> UpdatePaymentMethodProfile([FromBody]PaymentDetailModel model)
        {
            try
            {
                Response response = new Response();
                response.TransResponse = await new PaymentManager().UpdatePaymentMethodProfile(model);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}