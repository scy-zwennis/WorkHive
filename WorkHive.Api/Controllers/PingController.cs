using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WorkHive.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PingController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("Pong"); 
        }
    }
}
