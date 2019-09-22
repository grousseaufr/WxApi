using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using WxApi.Configuration.Settings;
using WxApi.Dtos.Products;
using WxApi.Services.Enums;
using WxApi.Services.Extensions;

namespace WxApi.Services
{
    public class ProductService : AbstractService<Product>, IProductService
    {
        private readonly IShopperHistoryService _shopperHistoryService;

        public ProductService(HttpClient httpClient, IUserService userService, IShopperHistoryService shopperHistoryService, IOptions<AppSettings> appSettings) : base(httpClient, userService, appSettings)
        {
            _shopperHistoryService = shopperHistoryService;
            RequestUri = "products";
        }

        public override string RequestUri { get; protected set; }

        public async Task<List<Product>> GetAllSorted(SortOptions sortOption)
        {
            var allProducts = await GetAll();

            if (sortOption == SortOptions.Recommended)
            {
                var shopperHistoryProducts = await _shopperHistoryService.GetProducts();
                return allProducts.ApplyRecommendedSort(shopperHistoryProducts);
            }
            else
            {
                return allProducts.ApplySort(sortOption);
            }
        }
    }
}
