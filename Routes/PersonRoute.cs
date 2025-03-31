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
            var route = app.MapGroup(prefix: "/person");

            route.MapPost(pattern: "",
            async (PersonRequest req, PersonContext context) =>
            {
                var person = new PersonModel(req.name);
                await context.AddAsync(person);
                await context.SaveChangesAsync();
            });

            route.MapGet(pattern: "",
            async (PersonContext context) =>
            {
                var people = await context.People.ToListAsync();
                return Results.Ok(people);
            });

            route.MapPut(pattern: "{id:guid}",
            async (Guid id, PersonRequest req, PersonContext context) =>
            {
                var person = await context.People.FirstOrDefaultAsync(x => x.Id == id);

                if (person == null)
                    return Results.NotFound();

                person.ChangeName(req.name);
                await context.SaveChangesAsync();

                return Results.Ok(person);
            });

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

            app.MapPost("/login", (UserCredentials credentials) =>
            {
                // Replace with proper user validation logic
                if (credentials.Username == "admin" && credentials.Password == "password")
                {
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var key = Encoding.UTF8.GetBytes("your-secret-key"); // Replace with your secret key
                    var tokenDescriptor = new SecurityTokenDescriptor
                    {
                        Subject = new ClaimsIdentity(new[]
                        {
                            new Claim(ClaimTypes.Name, credentials.Username)
                        }),
                        Expires = DateTime.UtcNow.AddHours(1),
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