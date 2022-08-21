using JWT.Algorithms;
using JWT.Builder;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<UsersController> _logger;

        public UsersController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, ILogger<UsersController> logger)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateUserRequest request)
        {
            var newUser = new IdentityUser(request.Email)
            {
                Email = request.Email,
            };

            var result = await _userManager.CreateAsync(newUser, request.Password);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return Ok(newUser.Id);
        }


        [HttpPost("token")]
        public async Task<IActionResult> GetToken(GetTokenRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            var isSuccess = await _userManager.CheckPasswordAsync(user, request.Password);

            if (!isSuccess)
            {
                return BadRequest("Incorrect password");
            }

            var token = JwtBuilder.Create()
              .WithAlgorithm(new HMACSHA256Algorithm())
              .WithSecret("secret1234554351231")
              .ExpirationTime(DateTimeOffset.UtcNow.AddHours(1).ToUnixTimeSeconds())
              .AddClaim("claim1", 0)
              .AddClaim("claim2", "claim2-value")
              .AddClaim(ClaimTypes.NameIdentifier, user.Id)
              .WithVerifySignature(true)
              .Encode();

            return Ok(token);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Test(string token)
        {
            var json = JwtBuilder.Create()
              .WithAlgorithm(new HMACSHA256Algorithm())
              .WithSecret("secret1234554351231")
              .MustVerifySignature()
              .Decode(token);

            var userIdClaim = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return BadRequest("Cannot get user Id from the token");
            }

            if (!Guid.TryParse(userIdClaim.Value, out var userId))
            {
                return BadRequest("Cannot get user Id from the token");
            }

            var user = _userManager.FindByIdAsync(userIdClaim.Value);

            return Ok(new { user, json });
        }
    }

    public class GetTokenRequest
    {
        [EmailAddress]
        public string Email { get; set; }

        public string Password { get; set; }

    }

    public class CreateUserRequest
    {
        [EmailAddress]
        public string Email { get; set; }

        public string Password { get; set; }

        [Compare(nameof(Password))]
        public string ConfirmationPassword { get; set; }
    }
}
