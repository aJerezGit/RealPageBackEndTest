  
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using webapiRealPage.Models;

namespace webapiRealPage.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserProfileController : ControllerBase
    {
        private UserManager<ApplicationUser> _userManager;
        private readonly ILogger<ApplicationUser> _logger;
        public UserProfileController(UserManager<ApplicationUser> userManager, ILogger<ApplicationUser> logger)
        {
            _userManager = userManager;
            _logger = logger;
            _logger.LogDebug(1, "NLog injected into User Profile");
        }

        [HttpGet]
        // [Authorize]
        //GET : /api/UserProfile
        public async Task<Object> GetUserProfile() {
            // var test = User.Claims.First().Value;
            string userId = User.Claims.First(c => c.Type == "UserID").Value;
            var user = await _userManager.FindByIdAsync(userId);
            _logger.LogInformation(DateTime.Now.ToString() +  " - " + "UserAction: " + "Read Info request JWT");
            return new
            {
                 user.FullName,
                 user.Email,
                 user.UserName,
                 user.Admin
            };
        }
    }
}