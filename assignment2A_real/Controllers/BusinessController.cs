using Microsoft.AspNetCore.Mvc;

namespace assignment2A_real.Controllers
{
    [Route("api")] 
    [ApiController]
    public class BusinessController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            var message = "Welcome to the Business Layer";
            return Ok(message);
        }
    }
}
