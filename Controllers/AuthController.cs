using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ProductApi.Models.DTO;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace ProductApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMailService _mailService;
        private readonly IProductService _productService;

        public AuthController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager,
            RoleManager<IdentityRole> roleManager, IMailService mailService, IProductService productService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _productService = productService;
            
            _mailService = mailService;
        }

        [HttpGet("migrate")]
        public IActionResult Migrate()
        {
            ApplicationDbContext.SeedData(_userManager, _roleManager, _productService).Wait();
            return Ok();
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody]UserCreateDto user)
        {

            AppUser newUser = new AppUser
            {
                UserName = user.UserName,
                Email = user.Email,
                EmailConfirmed = false,
            };
            var result = await _userManager.CreateAsync(newUser, user.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(newUser, "User");
                return Ok();
            }
            else return Unauthorized();
        }

        [HttpGet("conformEmail")]
        public async Task<IActionResult> ConfirmEmail(string address)
        {
            await _mailService.SendConfirmationMessage(address);
            return Ok();
        }

        [HttpPost("conformEmail")]
        public async Task<IActionResult> ConfirmEmail(string address, string key){
            if(await _mailService.Confirm(address, key))
                return Ok("Your email successfully confirmed, so you can sign in to the site.");
           
            return BadRequest("Confirmation key isn't correct!");
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody]UserLoginDto login)
        {
            AppUser user = await _userManager.FindByNameAsync(login.UserName);
            if (user == null) return Unauthorized(new UserLoginResponseDto { IsAuthSuccessful = false,
                ErrorMessage =new List<string>() { "User with this login not found" }
            });
            
            await _signInManager.SignOutAsync();
            var signInResult = await _signInManager.PasswordSignInAsync(user, login.Password, false, false);
            if (!signInResult.Succeeded) return Unauthorized(new UserLoginResponseDto
            {
                IsAuthSuccessful = false,
                ErrorMessage = { "This password is invalid" }
            });

            List<Claim> claims = new List<Claim> { new Claim(ClaimTypes.Name, user.UserName) };
            foreach (var role in _roleManager.Roles)
            {
                if (await _userManager.IsInRoleAsync(user, role.Name))
                    claims.Add(new Claim(ClaimTypes.Role, role.Name));
            }
            
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("superSecretKey@345"));
            var signInCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
            var tokenOptions = new JwtSecurityToken(
                issuer: "https://localhost:44342",
                audience: "https://localhost:44342",
                claims: claims,
                expires: DateTime.Now.AddMinutes(5),
                signingCredentials: signInCredentials
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
            return Ok(new UserLoginResponseDto { Token = tokenString });
        }

        [HttpPost("logout")]
        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return Ok(); 
        }

    }
}
