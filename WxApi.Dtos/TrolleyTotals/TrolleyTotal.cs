using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WxApi.Dtos.Products;
using WxApi.Dtos.TrolleyTotal;

namespace WxApi.Dtos.TrolleyTotals
{
    public class TrolleyTotal
    {
        [Required]
        public List<Product> Products { get; set; }

        [Required]
        public List<Specials> Specials { get; set; }

        [Required]
        public List<Quantities> Quantities { get; set; }

        public bool HasSpecials
        {
            get
                {
                    return Specials != null && Specials.Count > 0;
                }
        }
    }
}
