using Microsoft.AspNetCore.Mvc;
using assignment2A_real.Data;
using assignment2A_real.Models;

namespace assignment2A_real.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserProfileController : Controller
    {

        // GET: api/UserProfiles
        [HttpGet]
        public ActionResult<IEnumerable<UserProfile>> GetAllUserProfiles()
        {
            var userProfiles = UserProfileManager.GetAllUserProfiles();
            return Ok(userProfiles);
        }

        [HttpPost]
        [Route("createUserProfile")]
        public IActionResult CreateUserProfile(UserProfile userProfile)
        {
            UserProfileManager.InsertUserProfile(userProfile);

            return Ok("User profile created successfully.");
        }

        [HttpGet("{username}")]
        public IActionResult GetUserProfileByUsername(string username)
        {
            var userProfile = UserProfileManager.GetUserProfileByUsername(username);

            if (userProfile == null)
            {
                return NotFound("User profile not found.");
            }

            return Ok(userProfile);
        }

        [HttpGet("byemail/{email}")]
        public IActionResult GetUserProfileByEmail(string email)
        {
            var userProfile = UserProfileManager.GetUserProfileByEmail(email);

            if (userProfile == null)
            {
                return NotFound("User profile not found.");
            }

            return Ok(userProfile);
        }

        [HttpPut("{username}")]
        public IActionResult UpdateUserProfile(string username, UserProfile updatedUserProfile)
        {
            if (!UserProfileManager.UserProfileExists(username))
            {
                return NotFound("User profile not found.");
            }

            UserProfileManager.UpdateUserProfile(username, updatedUserProfile);
            return Ok("User profile updated successfully.");
        }
    }
}
