using System.Collections.Generic;

namespace WxApi.Dtos.Products
{
    public class ShopperHistory
    {
        public int CustomerId { get; set; }
        public List<Product> Products { get; set; }
    }
}