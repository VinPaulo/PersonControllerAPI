using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Person.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Person.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthController : ControllerBase // No [Authorize] here to allow public access to /login
    {
        [HttpPost("login")]
        public IActionResult Login([FromBody] UserCredentials credentials)
        {
            // Replace with proper user validation logic
            if (credentials.Username == "admin" && credentials.Password == "password")
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.UTF8.GetBytes("your-very-secure-and-long-secret-key"); // Ensure the key is at least 32 characters
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new[]
                    {
                        new Claim(ClaimTypes.Name, credentials.Username)
                    }),
                    Expires = DateTime.UtcNow.AddHours(1), // Ensure this is set to a reasonable value
                    Audience = "your-audience", // Ensure this matches ValidAudience in Program.cs
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                return Ok(new { Token = tokenHandler.WriteToken(token) });
            }

            return Unauthorized();
        }
    }
}
