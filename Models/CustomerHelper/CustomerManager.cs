using PaymentGateway;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Models.DynamicResponse;
using WebApi.Models.EbizModelHelper;

namespace WebApi.Models.CustomerHelper
{
    public class CustomerManager
    {
        //security token defined here
        public async Task<SecurityToken> GetSecurityToken()
        {
            SecurityToken securityToken = new SecurityToken();
            await Task.Run(() =>
            {
                securityToken.SecurityId = "99359f03-b254-4adf-b446-24957fcb46cb";
                securityToken.UserId = "qbouser";
                securityToken.Password = "mW64newLSvu!!!!";
            });
            return securityToken;
        }
        //Add New Customer
        public async Task<dynamic> AddNewCustomer(Customer customer)
        {
            Response response = new Response();
            SecurityToken token = await GetSecurityToken();
            var client = new PaymentGateway.IeBizServiceClient();
            response.CustomerResponse = await client.AddCustomerAsync(token, customer);
            return response.CustomerResponse;
        }

        //Get Customer
        public async Task<dynamic> GetCustomerDetails(CustomerDetailModel model)
        {
            SecurityToken token = await GetSecurityToken();
            var client = new PaymentGateway.IeBizServiceClient();
            var data = await client.GetCustomerAsync(token, model.customerId, model.customerInternalId);
            return data;
        }
        //Search All Customer
        public async Task<dynamic> SearchCustomerDetails(CustomerDetailModel model)
        {
            SecurityToken token = await GetSecurityToken();
            var client = new PaymentGateway.IeBizServiceClient();
            List<CustomerReturnModel> customerReturnModels = new List<CustomerReturnModel>();
            int length = 0;
            int lengthnode = 0;
            do
            {
                lengthnode++;
                var data = await client.SearchCustomerListAsync(token, model.searchFilters, model.start, model.limit, model.sort, model.includeCustomerToken, model.includePaymentMethodProfile, model.countOnly);
                length = data.Count;
                CustomerReturnModel customerReturn = new CustomerReturnModel();
                customerReturn.customers = data.CustomerList.ToList();
                customerReturnModels.Add(customerReturn);
                model.start = Int32.Parse(lengthnode + "000");
                model.limit = Int32.Parse(lengthnode + "999");
            } while (length >= 1000);
            return customerReturnModels;
        }
        //Search Customer with pagination
        public async Task<dynamic> SearchCustomersWithPagination(CustomerDetailModel model)
        {
            SecurityToken token = await GetSecurityToken();
            var client = new PaymentGateway.IeBizServiceClient();
            model.start = (model.pageno - 1) * model.pagesize;
            model.limit = model.pagesize;
            var data = await client.SearchCustomerListAsync(token, model.searchFilters, model.start, model.limit, model.sort, model.includeCustomerToken, model.includePaymentMethodProfile, model.countOnly);
            return data;
        }
    }
}
