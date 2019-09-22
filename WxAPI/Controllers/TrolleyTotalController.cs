using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WxApi.Dtos.TrolleyTotals;
using WxApi.Services;

namespace WxApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrolleyTotalController : ControllerBase
    {
        private readonly ITrolleyCalculatorService _trolleyCalculatorService;

        public TrolleyTotalController(ITrolleyCalculatorService trolleyCalculatorService)
        {
            _trolleyCalculatorService = trolleyCalculatorService;
        }

        // Version without relying on /resource/trolleyCalculator api
        //[HttpPost]
        //public ActionResult Post([FromBody] TrolleyTotal trolleyTotal)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest();
        //    }

        //    try
        //    {
        //        var total = _trolleyCalculatorService.GetMinTrolleyTotal(trolleyTotal);

        //        return Ok(total);
        //    }
        //    catch (ArgumentException e)
        //    {
        //        return BadRequest(e);
        //    }
        //}

        // Version using /resource/trolleyCalculator api
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] TrolleyTotal trolleyTotal)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            try
            {
                var totalResponse = await _trolleyCalculatorService.Post(trolleyTotal);
                var total = decimal.Parse(totalResponse, CultureInfo.InvariantCulture);

                return Ok(total);
            }
            catch (ArgumentException e)
            {
                return BadRequest(e);
            }
        }
    }
}