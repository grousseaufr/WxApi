using System;
using System.Collections.Generic;
using System.Linq;
using WxApi.Dtos.Products;
using WxApi.Services.Enums;

namespace WxApi.Services.Extensions
{
    public static class SortListExtensions
    {
        public static List<Product> ApplySort(this List<Product> products, SortOptions sortOption)
        {
            switch (sortOption)
            {
                case SortOptions.Low:
                    return products.OrderBy(o => o.Price).ToList();
                case SortOptions.High:
                    return products.OrderByDescending(o => o.Price).ToList();
                case SortOptions.Ascending:
                    return products.OrderBy(o => o.Name).ToList();
                case SortOptions.Descending:
                    return products.OrderByDescending(o => o.Name).ToList();
                default:
                    throw new ArgumentException("Unknown sort option");
            }
        }

        public static List<Product> ApplyRecommendedSort(this List<Product> allProducts, List<Product> shopperHistoryProducts)
        {
            //Group by name, Sum quantity
            var recommandedProducts = shopperHistoryProducts.GroupBy(g => g.Name)
                                                            .Select(s => new 
                                                            {
                                                                Name = s.Key,
                                                                Quantity = s.Sum(su => su.Quantity),
                                                            }).ToList();

            //update product quantity in allProduct list
            foreach (var recommandedProduct in recommandedProducts)
            {
                allProducts.First(f => f.Name == recommandedProduct.Name).Quantity = recommandedProduct.Quantity;
            }

            return allProducts.OrderByDescending(o => o.Quantity).ToList();
        }
    }
}
