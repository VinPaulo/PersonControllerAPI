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
    public class AuthController : ControllerBase // permitir acesso público ao endpoint /login
    {
        [HttpPost("login")]
        public IActionResult Login([FromBody] UserCredentials credentials)
        {
            if (credentials.Username == "admin" && credentials.Password == "password")
            {
                var tokenHandler = new JwtSecurityTokenHandler();

                // Chave secreta usada para assinar o token (deve ter pelo menos 32 caracteres)
                var key = Encoding.UTF8.GetBytes("minhasupersecretaechavecom32caracteres");

                // Configuração do token JWT
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new[]
                    {
                        // Adiciona o nome do usuário como uma reivindicação (claim)
                        new Claim(ClaimTypes.Name, credentials.Username)
                    }),
                    Expires = DateTime.UtcNow.AddHours(1), // Define a expiração do token para 1 hora
                    Issuer = "https://meusistema.com",
                    Audience = "https://meusistema.com", // Deve corresponder ao ValidAudience configurado no Program.cs
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature) // Assinatura do token
                };

                // Cria o token JWT
                var token = tokenHandler.CreateToken(tokenDescriptor);

                // Retorna o token gerado no corpo da resposta
                return Ok(new { Token = tokenHandler.WriteToken(token) });
            }

            // Retorna 401 Unauthorized se as credenciais forem inválidas
            return Unauthorized();
        }
    }
}
