using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PaymentGateway;
using WebApi.Models.SyncHelper;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SyncController : ControllerBase
    {
        [Route("[action]")]
        [HttpPost]
        public async Task<IActionResult> UploadCustomers([FromBody]SyncDetailModel model)
        {
            var client = new PaymentGateway.IeBizServiceClient();
            try
            {
                List<Customer> customers = new List<Customer>();
                Customer customer1 = new Customer();
                customer1.FirstName = "Mark";
                customer1.LastName = "Wilson";
                customer1.CompanyName = "CBS";
                customer1.CustomerId = "C-E&000001";
                customer1.CellPhone = "714-555-5014";
                customer1.Fax = "714-555-5010";
                customer1.Phone = "714-555-5015";
                customer1.BillingAddress = new Address();
                customer1.BillingAddress.Address1 = "20 Pacifica";
                customer1.BillingAddress.Address2 = "Suite 1450";
                customer1.BillingAddress.City = "Irvine";
                customer1.BillingAddress.ZipCode = "92618";
                customer1.BillingAddress.State = "CA";
                customers.Add(customer1);
                Customer customer2 = new Customer();
                customer2.FirstName = "Mark";
                customer2.LastName = "Wilson";
                customer2.CompanyName = "CBS";
                customer2.CustomerId = "C-E&000002";
                customer2.CellPhone = "714-555-5014";
                customer2.Fax = "714-555-5010";
                customer2.Phone = "714-555-5015";
                customer2.BillingAddress = new Address();
                customer2.BillingAddress.Address1 = "20 Pacifica";
                customer2.BillingAddress.Address2 = "Suite 1450";
                customer2.BillingAddress.City = "Irvine";
                customer2.BillingAddress.ZipCode = "92618";
                customer2.BillingAddress.State = "CA";
                customers.Add(customer2);
                model.customers = customers;
                dynamic response = await new SyncManager().SyncCustomers(model);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Route("[action]")]
        [HttpPost]
        public async Task<IActionResult> UploadSalesOrder([FromBody]SyncDetailModel model)
        {
            var client = new PaymentGateway.IeBizServiceClient();
            try
            {
                List<SalesOrder> orders = new List<SalesOrder>();
                SalesOrder s1 = new SalesOrder();
                s1.CustomerId = "C-E&000001";
                s1.Amount = 125;
                s1.DueDate = DateTime.Now.ToString();
                s1.AmountDue = 100;
                s1.PoNum = "po001001";
                s1.SalesOrderNumber = "So107001";
                Item[] items = new Item[2];
                Item item = new Item();
                Item item1 = new Item();
                item.ItemId = "01-00099";
                item.Name = "New Books test";
                item.Description = "Naqi Items new";
                item.UnitPrice = 50;
                item.Qty = 2;
                item.TotalLineAmount = 100;
                item.TotalLineTax = 10;
                item.ItemLineNumber = 1;
                items[0] = item;
                item1.ItemId = "01-00098";
                item1.Name = "New Books";
                item1.Description = "Naqi Items";
                item1.UnitPrice = 50;
                item1.Qty = 2;
                item1.TotalLineAmount = 100;
                item1.TotalLineTax = 10;
                item1.ItemLineNumber = 1;
                items[1] = item1;
                s1.Items = items;
                SalesOrder s2 = new SalesOrder();
                s2.CustomerId = "C-E&000002";
                s2.Amount = 130;
                s2.DueDate = DateTime.Now.ToString();
                s2.AmountDue = 100;
                s2.PoNum = "po001002";
                s2.SalesOrderNumber = "So107002";
                orders.Add(s1);
                orders.Add(s2);
                model.salesOrders = orders;
                dynamic response = await new SyncManager().SyncOrders(model);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        //[Route("[action]")]
        //[HttpPost]
        //public async Task<IActionResult> UploadInovoices([FromBody]SyncDetailModel model)
        //{
        //    try
        //    {
        //        dynamic response = await new SyncManager().ItemsSync(model);
        //        return Ok(response);
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}
        //[Route("[action]")]
        //[HttpPost]
        //public async Task<IActionResult> UploadItems([FromBody]SyncDetailModel model)
        //{
        //    try
        //    {
        //        dynamic response = await new SyncManager().ItemsSync(model);
        //        return Ok(response);
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}
    }
}