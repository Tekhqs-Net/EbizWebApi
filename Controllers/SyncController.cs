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
                //s1.CustomerId = "C-E&000001";
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
                //s2.CustomerId = "C-E&000002";
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
        [Route("[action]")]
        [HttpPost]
        public async Task<IActionResult> UploadInovoices([FromBody]SyncDetailModel model)
        {
            try
            {
                List<Invoice> invoices = new List<Invoice>();
                Invoice invoice = new Invoice();
                //invoice.CustomerId = "C-E&000002";
                invoice.SubCustomerId = "";
                invoice.InvoiceNumber = "1551";
                invoice.InvoiceDate = DateTime.Now.ToString();
                invoice.InvoiceDueDate = DateTime.Now.ToString();
                invoice.InvoiceAmount = (decimal)2000.45;
                invoice.AmountDue = (decimal)200.45;
                invoice.DivisionId = "001";
                invoice.PoNum = "Po001";
                invoice.SoNum = "";
                Item[] Lineitems = new Item[2];
                Item item = new Item();
                item.ItemId = "001";
                item.Name = "Oranges";
                item.Description = "Oranges";
                item.Qty = 1;
                item.UnitPrice = (decimal)200.25;
                item.UnitOfMeasure = "EA";
                item.Taxable = true;
                item.TaxRate = (decimal)8.00;
                item.TotalLineTax = (decimal)5.25;
                item.TotalLineAmount = (decimal)205.25;
                item.ItemLineNumber = 1;
                Lineitems[0] = item;
                Item item2 = new Item();
                item2.ItemId = "N101";
                item2.Name = "CBS101100";
                item2.Qty = 1;
                item2.UnitPrice = 2000;
                Lineitems[1] = item2;
                invoice.Items = Lineitems;
                Invoice invoice2 = new Invoice();
                //invoice2.CustomerId = "C-E&000002";
                invoice2.SubCustomerId = "";
                invoice2.InvoiceNumber = "1552";
                invoice2.InvoiceDate = DateTime.Now.ToString();
                invoice2.InvoiceDueDate = DateTime.Now.ToString();
                invoice2.InvoiceAmount = (decimal)2000.45;
                invoice2.AmountDue = (decimal)200.45;
                invoice2.DivisionId = "001";
                invoice2.PoNum = "Po001";
                invoice2.SoNum = "";
                Item[] Lineitems2 = new Item[1];
                Item item1 = new Item();
                item1.ItemId = "001";
                item1.Name = "Blacks";
                item1.Description = "Blacks";
                item1.Qty = 1;
                item1.UnitPrice = (decimal)200.25;
                item1.UnitOfMeasure = "EA";
                item1.Taxable = true;
                item1.TaxRate = (decimal)8.00;
                item1.TotalLineTax = (decimal)5.25;
                item1.TotalLineAmount = (decimal)205.25;
                item1.ItemLineNumber = 1;
                Lineitems2[0] = item1;
                invoice2.Items = Lineitems2;
                invoices.Add(invoice);
                invoices.Add(invoice2);
                model.invoices = invoices;
                dynamic response = await new SyncManager().SyncInvoices(model);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Route("[action]")]
        [HttpPost]
        public async Task<IActionResult> UploadItems([FromBody]SyncDetailModel model)
        {
            try
            {
                List<ItemDetails> itemDetails = new List<ItemDetails>();
                ItemDetails item = new ItemDetails();
                item.ItemId = "N003";
                item.Name = "Electrical Equipments";
                item.Description = "Monitors";
                item.UnitPrice = (decimal)200.25;
                item.UnitOfMeasure = "EA";
                item.Taxable = true;
                item.TaxRate = (decimal)8.00;
                ItemDetails item2 = new ItemDetails();
                item2.ItemId = "N004";
                item2.Name = "Electrical Equipments";
                item2.Description = "Monitors";
                item2.UnitPrice = (decimal)200.25;
                item2.UnitOfMeasure = "EA";
                item2.Taxable = true;
                item2.TaxRate = (decimal)8.00;
                itemDetails.Add(item);
                itemDetails.Add(item2);
                model.items = itemDetails;
                dynamic response = await new SyncManager().SyncItems(model);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}