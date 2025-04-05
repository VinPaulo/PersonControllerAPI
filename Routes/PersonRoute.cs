using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Person.Data;
using Person.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Person.Routes
{
    public static class PersonRoute
    {
        public static void PersonRoutes(this WebApplication app)
        {
            // Define a rota base para "/person"
            var route = app.MapGroup(prefix: "/person");

            // Endpoint para criar uma nova pessoa
            route.MapPost(pattern: "",
            async (PersonRequest req, PersonContext context) =>
            {
                var person = new PersonModel(req.Name);
                await context.AddAsync(person);
                await context.SaveChangesAsync();
                return Results.Created($"/person/{person.Id}", person); // Adicionando retorno adequado
            });

            // Endpoint para listar todas as pessoas
            route.MapGet(pattern: "",
            async (PersonContext context) =>
            {
                var people = await context.People.ToListAsync();
                return Results.Ok(people);
            });

            // Endpoint para atualizar o nome de uma pessoa pelo ID
            route.MapPut(pattern: "{id:guid}",
            async (Guid id, PersonRequest req, PersonContext context) =>
            {
                var person = await context.People.FirstOrDefaultAsync(x => x.Id == id);

                if (person == null)
                    return Results.NotFound();

                person.ChangeName(req.Name); 
                await context.SaveChangesAsync();

                return Results.Ok(person);
            });

            // Endpoint para desativar uma pessoa pelo ID
            route.MapDelete(pattern: "{id:guid}",
            async (Guid id, PersonContext context) =>
            {
                var person = await context.People.FirstOrDefaultAsync(x => x.Id == id);

                if (person == null)
                    return Results.NotFound();

                person.SetInactive();
                await context.SaveChangesAsync();

                return Results.Ok(person);
            });

            // Endpoint para login e geração de token JWT
            app.MapPost("/login", (UserCredentials credentials) =>
            {
                // Validação de modelo
                if (string.IsNullOrWhiteSpace(credentials.Username) || string.IsNullOrWhiteSpace(credentials.Password))
                {
                    return Results.BadRequest(new { message = "Username and password are required" });
                }

                // Substituir pela lógica de validação de usuário adequada
                if (credentials.Username == "admin" && credentials.Password == "password")
                {
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var key = Encoding.UTF8.GetBytes("minhasupersecretaechavecom32caracteres"); // Atualizando para usar a mesma chave
                    var tokenDescriptor = new SecurityTokenDescriptor
                    {
                        Subject = new ClaimsIdentity(new[]
                        {
                            new Claim(ClaimTypes.Name, credentials.Username)
                        }),
                        Expires = DateTime.UtcNow.AddHours(1),
                        Issuer = "https://meusistema.com", // Adicionando issuer consistente
                        Audience = "https://meusistema.com", // Adicionando audience consistente
                        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                    };

                    var token = tokenHandler.CreateToken(tokenDescriptor);
                    return Results.Ok(new { Token = tokenHandler.WriteToken(token) });
                }

                return Results.Unauthorized();
            });
        }
    }
}