using Blog.API.Models.DTO;
using Blog.API.Repositories.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;


namespace Blog.API.Controllers
{
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly ITokenRepository tokenRepository;

        public AuthController(UserManager<IdentityUser> userManager, ITokenRepository tokenRepository)
        {
            this.userManager = userManager;
            this.tokenRepository = tokenRepository;
        }

        //POST: /api/auth/login
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO requestDTO) {

            //Check Emial
            var identityUser =  await userManager.FindByEmailAsync(requestDTO.Email);

            if (identityUser is not null)
            {
                //Check Password
                var checkPasswordResult = await userManager.CheckPasswordAsync(identityUser, requestDTO.Password);

                if (checkPasswordResult)
                {
                    var roles = await userManager.GetRolesAsync(identityUser);

                    //Create Token
                    var jwtToken = tokenRepository.CreateJWTToken(identityUser, roles.ToList());

                    var res = new LoginResponseDTO()
                    {
                        Email = requestDTO.Email,
                        Roles = roles.ToList(),
                        Token = jwtToken,
                    };

                    return Ok(res);
                }
            }
            ModelState.AddModelError("", "Email or Password is Incorrect");

            return ValidationProblem(ModelState);
        }

        //POST: /api/auth/register
        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDTO requestDTO) {
            var user = new IdentityUser()
            {
                UserName = requestDTO.Email?.Trim(),
                Email = requestDTO.Email?.Trim(),
            };

            //Create User
            var identityResult = await userManager.CreateAsync(user, requestDTO.Password);

            if (identityResult.Succeeded)
            {
                // Add Role to user (Reader)
                identityResult = await userManager.AddToRoleAsync(user, "Reader");

                if (identityResult.Succeeded)
                {
                    return Ok();
                }
            }
            else
            {
                if (identityResult.Errors.Any())
                {
                    foreach (var error in identityResult.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }

            return ValidationProblem(ModelState);
        }
       
    }
}

