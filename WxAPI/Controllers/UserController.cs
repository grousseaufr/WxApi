using Microsoft.AspNetCore.Mvc;
using WxApi.Services;

namespace WxAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public ActionResult Get()
        {
            var user = _userService.Get();
            return Ok(user);
        }
    }
}
