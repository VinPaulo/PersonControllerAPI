using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Person.Data;
using Person.Models;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Person.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly PersonContext _context;
        private readonly string _jwtKey = "minhasupersecretaechavecom32caracteres";
        private readonly string _issuer = "https://meusistema.com";
        private readonly string _audience = "https://meusistema.com";

        public AuthController(PersonContext context)
        {
            _context = context;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] UserCredentials credentials)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = _context.Users.SingleOrDefault(u => u.Username == credentials.Username);

            if (user == null || !VerifyPasswordHash(credentials.Password, user.PasswordHash))
            {
                return Unauthorized();
            }

            var token = GenerateJwtToken(credentials.Username);
            return Ok(new { Token = token });
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] UserCredentials credentials)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { message = "Validation failed", errors = ModelState });
            }

            if (string.IsNullOrWhiteSpace(credentials.Username))
            {
                return BadRequest(new { message = "Username cannot be empty" });
            }

            if (string.IsNullOrWhiteSpace(credentials.Password))
            {
                return BadRequest(new { message = "Password cannot be empty" });
            }

            if (_context.Users.Any(u => u.Username == credentials.Username))
            {
                return BadRequest("Username already exists.");
            }

            var user = new User
            {
                Username = credentials.Username,
                PasswordHash = HashPassword(credentials.Password)
            };

            _context.Users.Add(user);
            _context.SaveChanges();

            return Ok("User registered successfully.");
        }

        private string GenerateJwtToken(string username)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_jwtKey);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, username) }),
                Expires = DateTime.UtcNow.AddHours(1),
                Issuer = _issuer,
                Audience = _audience,
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key), 
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private string HashPassword(string password)
        {
            // Use a fixed salt for all passwords
            byte[] salt = Encoding.UTF8.GetBytes("SuperSecretSaltOnePiece2025.");
            
            // Use PBKDF2 algorithm with many iterations
            using var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000, HashAlgorithmName.SHA256);
            byte[] hash = pbkdf2.GetBytes(32);
            
            return Convert.ToBase64String(hash);
        }

        private bool VerifyPasswordHash(string password, string storedHash)
        {
            // Use the same salt and parameters as in HashPassword
            byte[] salt = Encoding.UTF8.GetBytes("SuperSecretSaltOnePiece2025.");
            
            using var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000, HashAlgorithmName.SHA256);
            byte[] hash = pbkdf2.GetBytes(32);
            
            return Convert.ToBase64String(hash) == storedHash;
        }
    }
}