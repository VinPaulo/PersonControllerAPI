using Microsoft.EntityFrameworkCore;
using Person.Data;
using Person.Models;

namespace Person.Routes
{
    // O "static" aqui significa que não precisa criar um objeto dessa classe pra usar os métodos
    public static class PersonEndpoints
    {
        // Esse método configura todas as rotas relacionadas a pessoas na nossa API
        public static void MapPersonEndpoints(this IEndpointRouteBuilder app)
        {
            // Aqui a gente cria um grupo de rotas, todas começando com "/person"
            // Isso deixa a API mais organizada e evita repetir "/person" em cada rota
            var personGroup = app.MapGroup("/person");

            // ROTA 1: Criar uma nova pessoa - POST /person/create
            personGroup.MapPost("/create", async (PersonRequest req, PersonContext context) =>
            {
                // Cria um objeto PersonModel com o nome recebido no request
                var person = new PersonModel(req.Name);
                // Adiciona no banco de dados
                await context.AddAsync(person);
                // Salva as mudanças
                await context.SaveChangesAsync();
                // Retorna status 201 (Created) com a pessoa criada e o link para acessá-la
                return Results.Created($"/person/getId/{person.Id}", person);
            });

            // ROTA 2: Listar todas as pessoas - GET /person/list
            personGroup.MapGet("/list", async (PersonContext context) =>
            {
                // Busca todas as pessoas do banco de dados
                var people = await context.People.ToListAsync();
                // Retorna a lista com status 200 (OK)
                return Results.Ok(people);
            });

            // ROTA 3: Atualizar uma pessoa pelo ID - PUT /person/insertId/{id}
            personGroup.MapPut("/insertId/{id:guid}", async (Guid id, PersonRequest req, PersonContext context) =>
            {
                // Busca a pessoa pelo ID
                var person = await context.People.FirstOrDefaultAsync(p => p.Id == id);

                // Se não encontrar, retorna 404 (Not Found)
                if (person == null)
                    return Results.NotFound();

                // Atualiza o nome da pessoa
                person.ChangeName(req.Name);
                // Salva as mudanças
                await context.SaveChangesAsync();

                // Retorna a pessoa atualizada com status 200 (OK)
                return Results.Ok(person);
            });

            // ROTA 4: Buscar uma pessoa pelo ID - GET /person/getId/{id}
            personGroup.MapGet("/getId/{id:guid}", async (Guid id, PersonContext context) =>
            {
                // Busca a pessoa pelo ID
                var person = await context.People.FirstOrDefaultAsync(p => p.Id == id);
                
                // Se não encontrar, retorna 404 (Not Found)
                if (person == null)
                    return Results.NotFound();
                
                // Retorna a pessoa encontrada com status 200 (OK)
                return Results.Ok(person);
            });

            // ROTA 5: "Deletar" uma pessoa (na verdade só marca como inativa) - DELETE /person/deleteId/{id}
            personGroup.MapDelete("/deleteId/{id:guid}", async (Guid id, PersonContext context) =>
            {
                // Busca a pessoa pelo ID
                var person = await context.People.FirstOrDefaultAsync(p => p.Id == id);

                // Se não encontrar, retorna 404 (Not Found)
                if (person == null)
                    return Results.NotFound();

                // Marca a pessoa como inativa (isso muda o nome para "desativado")
                person.SetInactive();
                // Salva as mudanças
                await context.SaveChangesAsync();

                // Retorna a pessoa (agora inativa) com status 200 (OK)
                return Results.Ok(person);
            });
        }
    }
}