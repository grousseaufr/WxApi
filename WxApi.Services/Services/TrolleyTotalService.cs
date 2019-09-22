using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using WxApi.Configuration.Settings;
using WxApi.Dtos.TrolleyTotal;
using WxApi.Dtos.TrolleyTotals;

namespace WxApi.Services
{
    public class TrolleyCalculatorService : AbstractService<TrolleyTotal>, ITrolleyCalculatorService
    {
        private TrolleyTotal _trolleyTotal;

        public TrolleyCalculatorService(HttpClient httpClient, IUserService userService, IOptions<AppSettings> appSettings) : base(httpClient, userService, appSettings)
        {
            RequestUri = "TrolleyCalculator";
        }

        public override string RequestUri { get; protected set; }

        public decimal GetMinTrolleyTotal(TrolleyTotal trolleyTotal)
        {
            _trolleyTotal = trolleyTotal;

            if (trolleyTotal.HasSpecials)
            {
                return GetWithSpecials();
            }

            return GetTotalWithoutSpecials();
        }

        private decimal GetTotalWithoutSpecials()
        {
            decimal total = 0;

            foreach (var product in _trolleyTotal.Products)
            {
                var quantityObject = _trolleyTotal.Quantities.FirstOrDefault(f => f.Name == product.Name);
                if(quantityObject != null)
                {
                    total += product.Price * quantityObject.Quantity;
                }
                else
                {
                    throw new ArgumentException($"Missing quantity for product {product.Name}");
                }
            }

            return total;
        }

        private decimal GetWithSpecials()
        {
            decimal total = 0;

            // Iterate on each specials
            foreach (var specialsObject in _trolleyTotal.Specials)
            {
                var matchCount = 0;
                var hasRemainingSpecials = true;

                while (hasRemainingSpecials)
                {
                    // Iterate on each quantities of current specials
                    foreach (var quantityObject in specialsObject.Quantities)
                    {
                        // Check if trolley has this product with enough quantity
                        var matchElement = _trolleyTotal.Quantities.Find(delegate (Quantities qu)
                        {
                            return qu.Name == quantityObject.Name && qu.Quantity >= quantityObject.Quantity;
                        });

                        if (matchElement != null)
                        {
                            matchCount++;
                        }
                    }

                    // If trolley has enough matching products, apply this specials
                    if (matchCount == specialsObject.Quantities.Count)
                    {
                        foreach (var q in _trolleyTotal.Quantities)
                        {
                            var specialQuantity = specialsObject.Quantities.FirstOrDefault(f => f.Name == q.Name);

                            if(specialQuantity != null)
                            {
                                //Remove product quantity that are part of this specials
                                q.Quantity -= specialQuantity.Quantity;
                            }
                        }

                        // Add price of specials to total
                        total += specialsObject.Total;
                        matchCount = 0;
                    }
                    else
                    {
                        hasRemainingSpecials = false;
                    }
                }
            }

            // Add missing products that are not part of any specials
            total += GetTotalWithoutSpecials();

            return total;
        }
    }
}
