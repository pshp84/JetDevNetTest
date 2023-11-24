using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using UserJourney.Common.CommonService;
using UserJourney.Common.DTOs;
using UserJourney.Data;

namespace UserJourney.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<AppUser> userManager;
        protected readonly JwtManager jwtManager;
        
        public AuthController(UserManager<AppUser> userManager, JwtManager jwtManager)
        {
            this.userManager = userManager;
            this.jwtManager = jwtManager;
        }


        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            try
            {
                var userExists = await userManager.FindByEmailAsync(model.Email);
                if (userExists != null)
                    return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User already exists." });

                AppUser user = new AppUser()
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    UserName = model.Email,
                    SecurityStamp = Guid.NewGuid().ToString(), 
                    EmailConfirmed = true,
                };
                var result = await userManager.CreateAsync(user, model.Password);
                if (!result.Succeeded)
                    return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User creation failed! Please check user details and try again." });

                return Ok(new Response { Status = "Success", Message = "User register successfully." });
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            try
            {
                var user = await userManager.FindByEmailAsync(model.Email);
                if (user == null)
                    return Ok(new Response { Status = "NotFound", Message = "Unable to login as user not exists in to system please sign up account." });

                else if(await userManager.CheckPasswordAsync(user, model.Password))
                {
                    var authClaims = new List<Claim>
                    { 
                        new Claim(ClaimTypes.Email, user.Email),
                        new Claim("UserId", user.Id),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    };

                    var token = jwtManager.GetToken(authClaims);

                    UserResDTO resDTO = new UserResDTO()
                    {
                        Id = user.Id,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Email = user.Email,
                    };

                    return Ok(new { token = token, user = resDTO, });
                }

                return Ok(new Response { Status = "Error", Message = "Email address or password is not validate." }); 
            }
            catch (Exception ex)
            {

                throw;
            }
        }


        [HttpPost("forgotPassword/{email}")]
        public async Task<IActionResult> ForgotPassword([EmailAddress] string email)
        {
            try
            {
                var user = await userManager.FindByEmailAsync(email);
                if (user == null)
                    return Ok(new Response { Status = "NotFound", Message = "Unable to login as user not exists in to system please sign up account." });

                var token = await userManager.GeneratePasswordResetTokenAsync(user);

                UserResDTO resDTO = new UserResDTO()
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                };

                return Ok(new { resetPasswordToken = token, user = resDTO });
            }

            catch (Exception ex)
            {
                throw;
            }
        }


        [HttpPost("resetPassword")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordModel model)
        {
            try
            {
                var user = await userManager.FindByEmailAsync(model.Email);
                if (user == null)
                    return Ok(new Response { Status = "NotFound", Message = "Unable to login as user not exists in to system please sign up account." });

                var resetPassResult = await userManager.ResetPasswordAsync(user, model.Token, model.Password);
                if (!resetPassResult.Succeeded)
                {
                    var lst = new List<string>();
                    foreach (var error in resetPassResult.Errors)
                    {
                        lst.Add(error.Description); 
                    }
                    return Ok(new Response { Status = "Error", Message = string.Join(",", lst) });
                }

                return Ok(new Response { Status = "Success", Message = "Password reset successfully." });
            }
            catch (Exception ex)
            {
                throw;
            }
        } 
    }
}
