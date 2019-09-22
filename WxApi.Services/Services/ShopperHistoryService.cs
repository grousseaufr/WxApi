using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using WxApi.Configuration.Settings;
using WxApi.Dtos.Products;

namespace WxApi.Services
{
    public class ShopperHistoryService : AbstractService<ShopperHistory>, IShopperHistoryService
    {
        public override string RequestUri { get; protected set; }

        public ShopperHistoryService(HttpClient httpClient, IUserService userService, IOptions<AppSettings> appSettings) : base(httpClient, userService, appSettings)
        {
            RequestUri = "shopperHistory";
        }

        public async Task<List<Product>> GetProducts()
        {
            var shopperHistory = await GetAll();

            if (shopperHistory.Any())
            {
                //Extract products
                var shopperHistoryProducts = shopperHistory.SelectMany(s => s.Products).ToList();
                return shopperHistoryProducts;
            }

            return null;
        }
    }
}
