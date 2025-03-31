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
                var person = new PersonModel(req.name); // Cria uma nova instância de PersonModel
                await context.AddAsync(person); // Adiciona a pessoa ao contexto
                await context.SaveChangesAsync(); // Salva as alterações no banco de dados
            });

            // Endpoint para listar todas as pessoas
            route.MapGet(pattern: "",
            async (PersonContext context) =>
            {
                var people = await context.People.ToListAsync(); // Obtém todas as pessoas do banco de dados
                return Results.Ok(people); // Retorna a lista de pessoas
            });

            // Endpoint para atualizar o nome de uma pessoa pelo ID
            route.MapPut(pattern: "{id:guid}",
            async (Guid id, PersonRequest req, PersonContext context) =>
            {
                var person = await context.People.FirstOrDefaultAsync(x => x.Id == id); // Busca a pessoa pelo ID

                if (person == null)
                    return Results.NotFound(); // Retorna 404 se a pessoa não for encontrada

                person.ChangeName(req.name); // Atualiza o nome da pessoa
                await context.SaveChangesAsync(); // Salva as alterações no banco de dados

                return Results.Ok(person); // Retorna a pessoa atualizada
            });

            // Endpoint para desativar uma pessoa pelo ID
            route.MapDelete(pattern: "{id:guid}",
            async (Guid id, PersonContext context) =>
            {
                var person = await context.People.FirstOrDefaultAsync(x => x.Id == id); // Busca a pessoa pelo ID

                if (person == null)
                    return Results.NotFound(); // Retorna 404 se a pessoa não for encontrada

                person.SetInactive(); // Marca a pessoa como inativa
                await context.SaveChangesAsync(); // Salva as alterações no banco de dados

                return Results.Ok(person); // Retorna a pessoa desativada
            });

            // Endpoint para login e geração de token JWT
            app.MapPost("/login", (UserCredentials credentials) =>
            {
                // Substituir pela lógica de validação de usuário adequada
                if (credentials.Username == "admin" && credentials.Password == "password")
                {
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var key = Encoding.UTF8.GetBytes("your-secret-key"); // Substituir pela chave secreta real
                    var tokenDescriptor = new SecurityTokenDescriptor
                    {
                        Subject = new ClaimsIdentity(new[]
                        {
                            new Claim(ClaimTypes.Name, credentials.Username) // Adiciona o nome do usuário como claim
                        }),
                        Expires = DateTime.UtcNow.AddHours(1), // Define a expiração do token
                        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature) // Define o algoritmo de assinatura
                    };

                    var token = tokenHandler.CreateToken(tokenDescriptor); // Cria o token JWT
                    return Results.Ok(new { Token = tokenHandler.WriteToken(token) }); // Retorna o token gerado
                }

                return Results.Unauthorized(); // Retorna 401 se as credenciais forem inválidas
            });
        }
    }
}