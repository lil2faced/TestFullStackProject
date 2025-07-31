using Microsoft.AspNetCore.Mvc;

namespace WebAPI.WebAPI.Controllers
{
    [Route("ping")]
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
