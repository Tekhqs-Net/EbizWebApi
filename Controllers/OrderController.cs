using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApi.Models.EbizModelHelper;
using WebApi.Models.OrderHelper;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        //Get  Order 
        [Route("[action]")]
        [HttpPost]
        public async Task<IActionResult> GetOrder([FromBody]OrderDetailModel model)
        {
            try
            {
                if (string.IsNullOrEmpty(model.salesOrderNo))
                {
                    string msg = "salesOrderNo should not be null or empty";
                    return BadRequest(msg);
                }
                dynamic response = await new OrderManager().GetOrderDetails(model);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        //Search all Orders 
        [Route("[action]")]
        [HttpPost]
        public async Task<IActionResult> SearchAllOrders([FromBody]OrderDetailModel model)
        {
            try
            {
                dynamic response = await new OrderManager().SearchAllOrders(model);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        //Search Orders with pagination
        [Route("[action]")]
        [HttpPost]
        public async Task<IActionResult> SearchOrdersWithPagination([FromBody]OrderDetailModel model)
        {
            try
            {
                if (string.IsNullOrEmpty(model.pageno.ToString()) && string.IsNullOrEmpty(model.pagesize.ToString()))
                {
                    string msg = "pageNo & pageSize should not be null or empty";
                    return BadRequest(msg);
                }
                dynamic response = await new OrderManager().SearchOrdersWithPagination(model);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}