using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using BCrypt.Net;
using System.Security.Claims;
using System.Text;
using First_Backend.Data;
using First_Backend.Models;
using First_Backend.Dtos;

namespace First_Backend.Controllers
{
    [ApiController] // Specified as an API controller
    [Route("api/auth")]
    public class AuthController: ControllerBase
    {
        private readonly MyDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthController(MyDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // Login the user
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            // Find the user by email
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == loginRequest.Email);
            if (user == null)
            {
                // Return Unauthorized for login error
                return Unauthorized("Invalid email or password.");
            }

            if (!BCrypt.Net.BCrypt.Verify(loginRequest.Password, user.Password))
            {
                // Return Unauthorized for wrong password
                return Unauthorized("Invalid email or password");
            }

            var token = GenerateJwtToken(user);
            return Ok(new { token });
        }

        // Generate JWT Token
        public string GenerateJwtToken(User user)
        {
            // Configuring JWT Authentication
            var jwtKey = _configuration["Jwt:Key"]; // JWT Key
            var jwtIssuer = _configuration["Jwt:Issuer"]; // JWT Issuer
            var jwtAudience = _configuration["Jwt:Audience"]; // JWT Audience

            // Check if the JWT config values are missing
            if (string.IsNullOrEmpty(jwtKey) || string.IsNullOrEmpty(jwtIssuer) || string.IsNullOrEmpty(jwtAudience))
            {
                throw new InvalidOperationException("JWT configuration values are missing");
            }

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256); // SHA 256

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer: jwtIssuer,
                audience: jwtAudience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(30), // 30 mins expiry
                signingCredentials: credentials);
            
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}