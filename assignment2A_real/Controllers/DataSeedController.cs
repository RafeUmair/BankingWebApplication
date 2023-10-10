using assignment2A_real.Data;
using Microsoft.AspNetCore.Mvc;
using System.Data.Entity;

namespace assignment2A_real.Controllers
{
    public class DataSeedController : ControllerBase
    {


        [HttpGet]
        [Route("create")]

        public IActionResult StartSeed()
        {
            AccountManager.SeedAccount();
            UserProfileManager.SeedUserProfile();
            TransactionManager.SeedTransaction();
            var response = new
            {
                Message = "Successfully loaded random data"
            };

            return Ok(response);
        }
    }
}
