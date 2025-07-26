using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Inkr.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {

        [HttpGet, Authorize]
        public IActionResult Test()
        {
            return Ok("You are a user");
        }

        [HttpGet("author"), Authorize(Roles = "author")]
        public IActionResult TestAdmin()
        {
            return Ok("You are an author");
        }

    }
}