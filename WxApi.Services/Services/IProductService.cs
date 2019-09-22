using System.Collections.Generic;
using System.Threading.Tasks;
using WxApi.Dtos.Products;
using WxApi.Services.Enums;

namespace WxApi.Services
{
    public interface IProductService
    {
        Task<List<Product>> GetAll();
        Task<List<Product>> GetAllSorted(SortOptions sortOption);
    }
}