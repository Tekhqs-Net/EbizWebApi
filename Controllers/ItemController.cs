using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApi.Models.EbizModelHelper;
using WebApi.Models.ItemHelper;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemController : ControllerBase
    {
        //Get  Item 
        [Route("[action]")]
        [HttpPost]
        public async Task<IActionResult> GetItem([FromBody]ItemDetailModel model)
        {
            try
            {
                dynamic response = await new ItemManager().GetItemDetails(model);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        //Search Item 
        [Route("[action]")]
        [HttpPost]
        public async Task<IActionResult> SearchAllItems([FromBody]ItemDetailModel model)
        {
            try
            {
                dynamic response = await new ItemManager().SearchAllItems(model);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        //Search Items with pagination
        [Route("[action]")]
        [HttpPost]
        public async Task<IActionResult> SearchItemsWithPagination([FromBody]ItemDetailModel model)
        {
            try
            {
                dynamic response = await new ItemManager().SearchItemsWithPagination(model);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}