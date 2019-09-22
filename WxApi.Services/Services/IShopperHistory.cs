using System.Collections.Generic;
using System.Threading.Tasks;
using WxApi.Dtos.Products;

namespace WxApi.Services
{
    public interface IShopperHistoryService
    {
        Task<List<ShopperHistory>> GetAll();
        Task<List<Product>> GetProducts();
    }
}