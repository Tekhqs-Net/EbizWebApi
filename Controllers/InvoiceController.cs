using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApi.Models.EbizModelHelper;
using WebApi.Models.InvoiceHelper;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvoiceController : ControllerBase
    {
        // Get Invoices
        [Route("[action]")]
        [HttpPost]
        public async Task<IActionResult> GetInvoice([FromBody]InvoiceDetailModel model)
        {
            try
            {
                dynamic response = await new InvoiceManager().GetInvoiceDetails(model);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        // Search Invoices
        [Route("[action]")]
        [HttpPost]
        public async Task<IActionResult> SearchAllInvoices([FromBody]InvoiceDetailModel model)
        {
            try
            {
                dynamic response = await new InvoiceManager().SearchAllInvoices(model);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        // Search Invoices with pagination
        [Route("[action]")]
        [HttpPost]
        public async Task<IActionResult> SearchInvoicesWithPagination([FromBody]InvoiceDetailModel model)
        {
            try
            {
                dynamic response = await new InvoiceManager().SearchInvoicesWithPagination(model);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}