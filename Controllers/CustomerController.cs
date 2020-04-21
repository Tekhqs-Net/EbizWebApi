using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApi.Models.CustomerHelper;
using WebApi.Models.EbizModelHelper;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        // Get Customer
        [Route("[action]")]
        [HttpPost]
        public async Task<IActionResult> GetCustomer([FromBody]CustomerDetailModel model)
        {
            try
            {
                if (string.IsNullOrEmpty(model.customerId))
                {
                    string msg = "customerId should not be null or empty";
                    return BadRequest(msg);
                }
                dynamic response = await new CustomerManager().GetCustomerDetails(model);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        // Search Customer
        [Route("[action]")]
        [HttpPost]
        public async Task<IActionResult> SearchAllCustomers([FromBody]CustomerDetailModel model)
        {
            try
            {
                dynamic response = await new CustomerManager().SearchCustomerDetails(model);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        // Search Customer with pagination
        [Route("[action]")]
        [HttpPost]
        public async Task<IActionResult> SearchCustomersWithPagination([FromBody]CustomerDetailModel model)
        {
            try
            {
                if (string.IsNullOrEmpty(model.pageno.ToString()) && string.IsNullOrEmpty(model.pagesize.ToString()))
                {
                    string msg = "pageNo & pageSize should not be null or empty";
                    return BadRequest(msg);
                }
                dynamic response = await new CustomerManager().SearchCustomersWithPagination(model);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}