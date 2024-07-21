using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace CrudSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly ILogger<AuthController> _logger;

        public AuthController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration, ILogger<AuthController> logger)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _logger = logger;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            _logger.LogInformation("Register endpoint called.");
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Model state is invalid.");
                return BadRequest(ModelState);
            }

            var user = new IdentityUser { UserName = model.Username, Email = model.Email };
            var result = await _userManager.CreateAsync(user, model.Password!);

            if (result.Succeeded)
            {
                _logger.LogInformation("User created successfully.");
                
                // Ensure the roles exist
                if (!await _roleManager.RoleExistsAsync("Admin"))
                {
                    await _roleManager.CreateAsync(new IdentityRole("Admin"));
                    Console.Write("i am admin");
                }
                if (!await _roleManager.RoleExistsAsync("User"))
                {
                    await _roleManager.CreateAsync(new IdentityRole("User"));
                     Console.Write("i am user");
                }

                // Assign the role to the user
                var roleResult = await _userManager.AddToRoleAsync(user, model.Role!); // Assuming model.Role contains either "Admin" or "User"
                if (!roleResult.Succeeded)
                {
                    _logger.LogError("Failed to assign role.");
                    return BadRequest(new { error = "Failed to assign role." });
                }
                return Ok(new { result = "User created successfully with role " + model.Role });
            }

            _logger.LogError("User creation failed.");
            foreach (var error in result.Errors)
            {
                _logger.LogError(error.Description);
            }

            return BadRequest(new { error = result.Errors });
        }

   [HttpPost("login")]
public async Task<IActionResult> Login([FromBody] LoginModel model)
{
    var user = await _userManager.FindByNameAsync(model.Username!);
    if (user == null)
    {
        return Unauthorized();
    }

    if (!await _userManager.CheckPasswordAsync(user, model.Password!))
    {
        return Unauthorized();
    }

    var userRoles = await _userManager.GetRolesAsync(user);
    var claims = new List<Claim>
    {
        new Claim(JwtRegisteredClaimNames.Sub, user.UserName!),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
    };

    foreach (var userRole in userRoles)
    {
        claims.Add(new Claim(ClaimTypes.Role, userRole));
    }

    var jwtKey = _configuration["Jwt:Key"];
    if (jwtKey == null)
    {
        throw new ArgumentNullException("Jwt:Key", "JWT key configuration is missing or null.");
    }

    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

    var token = new JwtSecurityToken(
        issuer: _configuration["Jwt:Issuer"],
        audience: _configuration["Jwt:Issuer"],
        claims: claims,
        expires: DateTime.Now.AddMinutes(30),
        signingCredentials: creds);
        Console.Write(claims);
    return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token) });
}


    public class RegisterModel
    {
        [Required]
        public string? Username { get; set; }
        [Required]
        public string? Email { get; set; }
        [Required]
        public string? Password { get; set; }
        [Required]
        public string? Role { get; set; } // Add this field to accept role during registration
    }

    public class LoginModel
    {
        [Required]
        public string? Username { get; set; }
        [Required]
        public string? Password { get; set; }
    }
}
}