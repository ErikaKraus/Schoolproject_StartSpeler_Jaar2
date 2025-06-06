using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using API.Models;
using API.Data;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System;
using System.Security.Claims;

namespace API.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthControllerAPI : ControllerBase
    {
        private readonly UserManager<Gebruiker> _userManager;
        private readonly RoleManager<IdentityRole<string>> _roleManager;
        private readonly SignInManager<Gebruiker> _signInManager;
        private readonly ILogger<AuthControllerAPI> _logger;
        private readonly IConfiguration _configuration;

        public AuthControllerAPI(UserManager<Gebruiker> userManager, RoleManager<IdentityRole<string>> roleManager, SignInManager<Gebruiker> signInManager, ILogger<AuthControllerAPI> logger, IConfiguration configuration)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _logger = logger;
            _configuration = configuration;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            var user = new Gebruiker
            {
                UserName = model.UserName,
                Email = model.Email,
                Naam = model.Naam,
                Voornaam = model.Voornaam
            };           


            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                //Rol toewijzing
                var roleExists = await _roleManager.RoleExistsAsync("speler");
                if (!roleExists)
                {
                    await _roleManager.CreateAsync(new IdentityRole<string>("speler"));
                }

                await _userManager.AddToRoleAsync(user, "speler");

                return Ok(new { Message = "Registratie gelukt en rol speler toegewezen" });
            }
            return BadRequest(result.Errors);

        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            _logger.LogInformation("Login attempt for user: {Email}", model.Email);
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                _logger.LogWarning("User not found: {Email}", model.Email);
                return Unauthorized();
            }

            var result = await _signInManager.PasswordSignInAsync(user.UserName, model.Password, model.RememberMe, lockoutOnFailure: false);
            if (!result.Succeeded)
            {
                _logger.LogWarning("Invalid password for user: {Email}", model.Email);
                return Unauthorized();
            }

            var token = GenerateJwtToken(user);
            _logger.LogInformation("Generated Token: {Token}", token); 
            return Ok(new { Token = token });
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            _logger.LogInformation("User logged out.");
            return Ok(new { Message = "User logged out successfully" });
        }

        private string GenerateJwtToken(Gebruiker user)
        {
            var claims = new[]
            {
        new Claim(JwtRegisteredClaimNames.Sub, user.Email),  // Ensure the email is set as the "sub" claim
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
    };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Issuer"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        [HttpGet("userinfo")]
        public async Task<IActionResult> GetUserInfo()
        {
            var userEmail = User.FindFirstValue(ClaimTypes.NameIdentifier);  //ClaimTypes.NameIdentifier om email uit te lezen
            if (string.IsNullOrEmpty(userEmail))
            {
                _logger.LogWarning("Email not found in token.");
                return NotFound("Email not found in token.");
            }

            _logger.LogInformation($"Email from token: {userEmail}");

            var user = await _userManager.FindByEmailAsync(userEmail);  //Gebruiker op email vinden
            if (user == null)
            {
                _logger.LogWarning($"User not found for email: {userEmail}");
                return NotFound("User not found.");
            }

            var userInfo = new
            {
                user.Id,
                user.UserName,
                user.Email,
                user.Naam,
                user.Voornaam
            };

            return Ok(userInfo);
        }






    }
}