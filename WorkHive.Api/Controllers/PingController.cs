using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WorkHive.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PingController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("Pong");
        }

        [HttpGet]
        [Route("Auth")]
        [Authorize]
        public IActionResult Auth()
        {
            return Ok();
        }
    }
}
