using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WxApi.Services;
using WxApi.Services.Enums;

namespace WxApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SortController : ControllerBase
    {
        private readonly IProductService _productService;

        public SortController(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<IActionResult> Get(string sortOption)
        {
            try
            {
                SortOptions sortOptionEnum;
                
                if(!Enum.TryParse(sortOption, true, out sortOptionEnum)){
                    return BadRequest("Unknown sort option");
                }

                var sortedProducts = await _productService.GetAllSorted(sortOptionEnum);
                return Ok(sortedProducts);
            }
            catch(ArgumentException e)
            {
                return BadRequest(e.Message);
            }
            catch(Exception e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }
        }
    }
}
