using Microsoft.AspNetCore.Mvc;

namespace assignment2A_real.Controllers
{
    [Route("api")] // Set the base route for this controller to "api"
    [ApiController]
    public class BusinessController : Controller
    {
        [HttpGet] // Create an action to represent the "localhost:port/api/" entry point
        public IActionResult Index()
        {
            var message = "Welcome to the Business Layer";
            return Ok(message);
        }
    }
}
