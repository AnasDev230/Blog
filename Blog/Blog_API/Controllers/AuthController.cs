using Blog_API.Models.DTO;
using Blog_API.Repositories.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Blog_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly ITokenRepository tokenRepository;

        public AuthController(UserManager<IdentityUser> userManager,ITokenRepository tokenRepository)
        {
            this.userManager = userManager;
            this.tokenRepository = tokenRepository;
        }
        [HttpPost]
        [Route("Register")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto request)
        {
            var identityUser = new IdentityUser
            {
                UserName=request.Username,
                Email=request.Username,
            };
            var identityResult = await userManager.CreateAsync(identityUser,request.Password);
            if (identityResult.Succeeded)
            { 
                if(request.Roles is not null && request.Roles.Any())
                identityResult=await userManager.AddToRolesAsync(identityUser,request.Roles);
                if(identityResult.Succeeded)
                {
                    return Ok("User Registered Successfully, pls Login");
                }
            }
            return BadRequest("Something Went Wrong!!");
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
        {
            var user = await userManager.FindByEmailAsync(request.Username);
            if (user is not null)
            {
                var PasswordResult=await userManager.CheckPasswordAsync(user, request.Password);
                if (PasswordResult)
                {
                    var roles=await userManager.GetRolesAsync(user);
                    if (roles!=null)
                    {
                        var jwtToken=await tokenRepository.CreateJWTToken(user, roles.ToList());
                        var response = new LoginResponseDto
                        {
                            JwtToken = jwtToken
                        };
                        return Ok(response);
                    }
                    
                }
            }
            return BadRequest("Username Or Password Incorrect!!");
        }

        
    }
}
