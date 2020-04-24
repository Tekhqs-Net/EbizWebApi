using PaymentGateway;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Models.EbizModelHelper;

namespace WebApi.Models.ItemHelper
{
    public class ItemManager
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
        //Get Item
        public async Task<dynamic> GetItemDetails(ItemDetailModel model)
        {
            SecurityToken token = await GetSecurityToken();
            var client = new PaymentGateway.IeBizServiceClient();
            var data = await client.SearchItemsAsync(token, model.itemInternalId, model.itemId, model.searchFilters, model.start, model.limit, model.sort);
            return data;
        }
        //Add Item
        public async Task<dynamic> AddItem(SecurityToken tokens, ItemDetails item)
        {
            SecurityToken token = await GetSecurityToken();
            var client = new PaymentGateway.IeBizServiceClient();
            var data = await client.AddItemAsync(token,item);
            return data;
        }
        //Update Item
        public async Task<dynamic> UpdateItemDetails(SecurityToken tokens, ItemDetails itemDetails)
        {
            SecurityToken token = await GetSecurityToken();
            var client = new PaymentGateway.IeBizServiceClient();
            var data = await client.UpdateItemAsync(token, itemDetails, itemDetails.ItemInternalId, itemDetails.ItemId);
            return data;
        }
        //Search Items
        public async Task<dynamic> SearchAllItems(ItemDetailModel model)
        {
            SecurityToken token = await GetSecurityToken();
            var client = new PaymentGateway.IeBizServiceClient();
            List<ItemReturnModel> itemReturnModels = new List<ItemReturnModel>();
            int length = 0;
            int lengthnode = 0;
            do
            {
                lengthnode++;
                var data = await client.SearchItemsAsync(token, model.itemInternalId, model.itemId, model.searchFilters, model.start, model.limit, model.sort);
                length = data.Length;
                ItemReturnModel itemReturn = new ItemReturnModel();
                itemReturn.items = data.ToList();
                itemReturnModels.Add(itemReturn);
                model.start = Int32.Parse(lengthnode + "000");
                model.limit = Int32.Parse(lengthnode + "999");
            } while (length >= 1000);
            return itemReturnModels;
        }
        //Search Items with Pagination
        public async Task<dynamic> SearchItemsWithPagination(ItemDetailModel model)
        {
            SecurityToken token = await GetSecurityToken();
            var client = new PaymentGateway.IeBizServiceClient();
            model.start = (model.pageno - 1) * model.pagesize;
            model.limit = model.pagesize;
            var data = await client.SearchItemsAsync(token, model.itemInternalId, model.itemId, model.searchFilters, model.start, model.limit, model.sort);
            return data;
        }
   
    }
}
