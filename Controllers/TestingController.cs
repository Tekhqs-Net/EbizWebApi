using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PaymentGateway;

namespace WebApi.Controllers
{
    public class TestingController : Controller
    {
        public async Task<IActionResult> Index()
        {
            //Live
            var client = new PaymentGateway.IeBizServiceClient();
            SecurityToken securityToken = new SecurityToken();
            //CreditCardData customer = new CreditCardData();
            //securityToken.UserId = "virtualterm";
            //securityToken.SecurityId = "8U5NF2nZQAm3G3Gw57oB8g7BObK7bzKP";
            //securityToken.Password = "xDvQuf2DuD!";
            securityToken.SecurityId = "99359f03-b254-4adf-b446-24957fcb46cb";
            securityToken.UserId = "qbouser";
            securityToken.Password = "mW64newLSvu!!!!";
            SearchFilter[] searchFilters = new SearchFilter[0];

            dynamic dsd = await client.SearchInvoicesAsync(securityToken,"","","","", searchFilters,0,200,"",false);
            PaymentMethodProfile payMethod = new PaymentMethodProfile();
            payMethod.CardExpiration = "0922";
            payMethod.CardNumber = "4000100011112224";
            payMethod.AvsStreet = "123 Main st.";
            payMethod.AvsZip = "90046";
            payMethod.MethodName = "My Visa";
            TransactionRequestObject transaction = new TransactionRequestObject();
            transaction.CreditCardData.CardNumber = "4000100011112224";
            transaction.CreditCardData.CardExpiration = "0922";
            transaction.CreditCardData.AvsStreet = "123 Main st.";
            transaction.CreditCardData.AvsZip = "90046";
            transaction.CustomerID = "";
            transaction.BillingAddress = new GwAddress {FirstName = "naqi", LastName = "ali" };
            transaction.Command = "AuthOnly";
            transaction.CustReceipt = true;
            transaction.CustReceiptName = "";
            transaction.AuthCode = "";
            var dd = await client.runTransactionAsync(securityToken,transaction);
            CustomerTransactionRequest customerTransaction = new CustomerTransactionRequest();
            var ss = await client.runCustomerTransactionAsync(securityToken, "C-ABC530", "1186", customerTransaction);
            //PaymentMethodProfile payMethod = new PaymentMethodProfile();
            //payMethod.CardExpiration = "1212";
            //payMethod.CardNumber = "4000100011112224";
            //payMethod.AvsStreet = "123 Main st.";
            //payMethod.AvsZip = "90046";
            //payMethod.MethodName = "My Visa";
            // dynamic data = await client.GetCustomerAsync(securityToken, "C-ABC527", "7c944faf-a409-460d-a55d-1a2220c455cd");
            //if (data!=null)
            //{

            //}
            //else
            //{
            //Customer customer = new Customer(); 
            //    customer.CompanyName = "Naqi101";
            //    customer.FirstName = "Naqi";
            //    customer.LastName = "Ali";
            //    customer.Email = "abc@yahoo.com";
            //    customer.SoftwareId = ".Net API";
            //    customer.CustomerId = "C-ABC530";
            //    Address address = new Address();
            //    address.FirstName = "Naqi";
            //    address.LastName = "Testing";
            //    address.CompanyName = "Naqi101";
            //    address.Address1 = "123 main st.";
            //    address.City = "Hollywood";
            //    address.State = "ca";
            //    address.ZipCode = "91607";
            //    address.Country = "USA";
            //    customer.BillingAddress = address;
            //    PaymentMethodProfile[] payMethod = new PaymentMethodProfile[1];
            //    payMethod[0] = new PaymentMethodProfile();
            //    //payMethod[0].MethodType = "1212";
            //    payMethod[0].CardNumber = "4000100011112224";
            //    payMethod[0].CardExpiration = "0922";
            //    payMethod[0].AccountHolderName = "Ali";
            //    payMethod[0].AvsStreet = "abc";
            //    payMethod[0].CardCode = "123";
            //    payMethod[0].AvsZip = "54000";
            //    payMethod[0].MethodName = "My Visa";
            //    customer.PaymentMethodProfiles = payMethod;
            //}

            try
            {
                var sdt = await client.AddCustomerPaymentMethodProfileAsync(securityToken, "4b9fab71-9b3c-4d77-bff3-fc6c4cf54ec0", payMethod);
                //dynamic response = await client.AddCustomerAsync(securityToken, customer);
                //dynamic response = await client.UpdateCustomerAsync(securityToken, customer, "C-ABC527", "7c944faf-a409-460d-a55d-1a2220c455cd");
                //MessageBox.Show(string.Concat(response));
            }

            catch (Exception err)
            {
                //MessageBox.Show(err.Message);
            }
            return View();
        }
        public async Task<IActionResult> Test()
        {
            //Live
            var client = new TestGateway.ueSoapServerPortTypeClient();
            TestGateway.ueSecurityToken token = new TestGateway.ueSecurityToken();
            token.SourceKey = "99359f03-b254-4adf-b446-24957fcb46cb";
            //token.ClientIP = "11.22.33.44";  // IP address of end user (if applicable)
            //string pin = "1234";   // pin assigned to source
            //SecurityToken securityToken = new SecurityToken();
            //securityToken.UserId = "vtuser";
            //securityToken.SecurityId = "8U5NF2nZQAm3G3Gw57oB8g7BObK7bzKP";
            //securityToken.Password = "vtuser1234";
            TestGateway.CustomerObject customer = new TestGateway.CustomerObject();
            TestGateway.Address address = new TestGateway.Address();
            address.FirstName = "Naqi";
            address.LastName = "Testing";
            address.Company = "Acme";
            address.Street = "123 main st.";
            address.City = "Hollywood";
            address.State = "ca";
            address.Zip = "91607";
            address.Country = "USA";
            customer.BillingAddress = address;

            customer.Enabled = true;
            customer.Amount = 5.00;
            customer.Next = "2010-08-15";
            customer.Schedule = "monthly";

            TestGateway.PaymentMethod[] payMethod = new TestGateway.PaymentMethod[1];
            payMethod[0] = new TestGateway.PaymentMethod();
            payMethod[0].CardExpiration = "1212";
            payMethod[0].CardNumber = "4444555566667779";
            payMethod[0].AvsStreet = "123 Main st.";
            payMethod[0].AvsZip = "90046";
            payMethod[0].MethodName = "My Visa";

            customer.PaymentMethods = payMethod;
            string response;

            try
            {
                response = await client.addCustomerAsync(token, customer);
                //MessageBox.Show(string.Concat(response));
            }

            catch (Exception err)
            {
                //MessageBox.Show(err.Message);
            }
            return View();
        }
    }
}