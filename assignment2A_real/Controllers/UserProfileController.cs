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
        public IActionResult CreateUserProfile(UserProfile userProfile)
        {
            // Insert the user profile into the database using UserProfileManager
            UserProfileManager.InsertUserProfile(userProfile);

            // Return a success response or appropriate status code
            return Ok("User profile created successfully.");
        }

        [HttpGet("{username}")]
        public IActionResult GetUserProfileByUsername(string username)
        {
            // Retrieve user profile by username using UserProfileManager
            var userProfile = UserProfileManager.GetUserProfileByUsername(username);

            if (userProfile == null)
            {
                return NotFound("User profile not found.");
            }

            // Return the user profile as a response
            return Ok(userProfile);
        }

        [HttpGet("byemail/{email}")]
        public IActionResult GetUserProfileByEmail(string email)
        {
            // Retrieve user profile by email using UserProfileManager
            var userProfile = UserProfileManager.GetUserProfileByEmail(email);

            if (userProfile == null)
            {
                return NotFound("User profile not found.");
            }

            // Return the user profile as a response
            return Ok(userProfile);
        }

        [HttpPut("{username}")]
        public IActionResult UpdateUserProfile(string username, UserProfile updatedUserProfile)
        {
            // Check if the user profile exists
            if (!UserProfileManager.UserProfileExists(username))
            {
                return NotFound("User profile not found.");
            }

            // Update the user profile
            //updatedUserProfile.Name = username; // Ensure the username is not changed
            UserProfileManager.UpdateUserProfile(username, updatedUserProfile);

            // Return a success response or appropriate status code
            return Ok("User profile updated successfully.");
        }
    }
}
