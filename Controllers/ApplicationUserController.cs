using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using webapiRealPage.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace webapiRealPage.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationUserController : ControllerBase {
         private UserManager<ApplicationUser> _userManager;
         private SignInManager<ApplicationUser> _signInManager;
         private readonly ApplicationSettings _appSettings;

        private readonly ILogger<ApplicationUser> _logger;

         public ApplicationUserController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IOptions<ApplicationSettings> appSettings, ILogger<ApplicationUser> logger)
         {
             _userManager = userManager;
             _signInManager = signInManager;
             _appSettings = appSettings.Value;
             _logger = logger;
            _logger.LogDebug(1, "NLog injected into ApplicationUser Controller");
         }

         [HttpPost]
         [Route("register")]
         //POST /api/ApplicationUser/Register
         public async Task<Object> PostApplicationUser(ApplicationUserModel model) {
             
             var applicationUser = new ApplicationUser() {
                 UserName = model.UserName,
                 Email = model.Email,
                 FullName = model.FullName
             };

             try
             {
                 var result = await _userManager.CreateAsync(applicationUser, model.Password);
                 _logger.LogInformation(DateTime.Now.ToString() +  " - " + "UserAction: " + "Created User: " + applicationUser.UserName + " Successfully");
                 return Ok(result);
             }
             catch(Exception ex)
             {
                 _logger.LogInformation(DateTime.Now.ToString() +  " - " + "UserAction: " + "Created User: " + applicationUser.UserName + " System Error");
                 _logger.LogError(DateTime.Now.ToString() +  " - " + "System Error" + ex.ToString());
                 throw ex;
             }    
         }

         [HttpPost]
        [Route("Login")]
        //POST : /api/ApplicationUser/Login
        public async Task<IActionResult> Login(LoginModel model)
        {

            var builder = new ConfigurationBuilder()
                                    .SetBasePath(Directory.GetCurrentDirectory())
                                    .AddJsonFile("appsettings.json");

            var configuration = builder.Build();

            string scaffoldJWTStr = configuration["ApplicationSettings:JTW_Secret"];
            
            var user = await _userManager.FindByNameAsync(model.UserName);
            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                        new Claim("UserID",user.Id.ToString())
                    }),
                    Expires = DateTime.UtcNow.AddMinutes(30),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(scaffoldJWTStr)), SecurityAlgorithms.HmacSha256Signature)
                };
                var tokenHandler = new JwtSecurityTokenHandler();
                var securityToken = tokenHandler.CreateToken(tokenDescriptor);
                var token = tokenHandler.WriteToken(securityToken);
                 _logger.LogInformation(DateTime.Now.ToString() +  " - " + "UserAction: " + " User Autentication: " + model.UserName + " Successfull JWT request.");
                return Ok(new { token });
            }
            else{
                _logger.LogInformation(DateTime.Now.ToString() +  " - " + "UserAction: " + " User Autentication: " + model.UserName + " Failed JWT request. Username or password Incorrect.");
                return BadRequest(new { message = "Username or password is incorrect." });
            }
                
        }
    }   
}