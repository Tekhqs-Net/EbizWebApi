using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApi.Models.RecurringHelper;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecurringBillingController : ControllerBase
    {
        [Route("[action]")]
        [HttpPost]
        public async Task<IActionResult> AddRecurringPayment([FromBody]RecurringDetailModel model)
        {
            try
            {
                dynamic response = await new RecurringManager().AddRecurringPayment(model);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Route("[action]")]
        [HttpPost]
        public async Task<IActionResult> UpdateRecurringPaymentStatus([FromBody]RecurringDetailModel model)
        {
            try
            {
                dynamic response = await new RecurringManager().ModifyScheduledRecurringPaymentStatus(model);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Route("[action]")]
        [HttpPost]
        public async Task<IActionResult> DeleteRecurringPayments([FromBody]RecurringDetailModel model)
        {
            try
            {
                dynamic response = await new RecurringManager().DeleteRecurringPayments(model);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Route("[action]")]
        [HttpPost]
        public async Task<IActionResult> SearchScheduledRecurringPayments([FromBody]RecurringDetailModel model)
        {
            try
            {
                dynamic response = await new RecurringManager().SearchScheduledRecurringPayments(model);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Route("[action]")]
        [HttpPost]
        public async Task<IActionResult> SearchReceivedRecurringPayments([FromBody]RecurringDetailModel model)
        {
            try
            {
                dynamic response = await new RecurringManager().SearchScheduledRecurringPayments(model);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}