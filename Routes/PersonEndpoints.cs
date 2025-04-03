using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Person.Data;
using Person.Models;
using Npgsql.EntityFrameworkCore.PostgreSQL;

namespace Person.Routes
{
    public static class PersonEndpoints
    {
        public static void MapPersonEndpoints(this IEndpointRouteBuilder app)
        {
            var personGroup = app.MapGroup("/person");

            personGroup.MapPost("/create", async (PersonRequest req, PersonContext context) =>
            {
                var person = new PersonModel(req.name);
                await context.AddAsync(person);
                await context.SaveChangesAsync();
                return Results.Ok(person);
            });

            personGroup.MapGet("/list", async (PersonContext context) =>
            {
                var people = await context.People.ToListAsync();
                return Results.Ok(people);
            });

            personGroup.MapPut("/insertId{id:guid}", async (Guid id, PersonRequest req, PersonContext context) =>
            {
                var person = await context.People.FirstOrDefaultAsync(p => p.Id == id);

                if (person == null)
                    return Results.NotFound();

                person.ChangeName(req.name);
                await context.SaveChangesAsync();

                return Results.Ok(person);
            });

            personGroup.MapGet("/getId{id:guid}", async (Guid id, PersonContext context) =>
            {
                var person = await context.People.FirstOrDefaultAsync(p => p.Id == id);
                return Results.Ok(person);
            });

            personGroup.MapDelete("/deleteId{id:guid}", async (Guid id, PersonContext context) =>
            {
                var person = await context.People.FirstOrDefaultAsync(p => p.Id == id);

                if (person == null)
                    return Results.NotFound();

                person.SetInactive();
                await context.SaveChangesAsync();

                return Results.Ok(person);
            });
        }
    }
}
