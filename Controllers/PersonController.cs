using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Person.Data;
using Person.Models;

namespace Person.Controllers
{
    [ApiController]
    [Route("person")]
    [Authorize] // Certifique-se de que esta anotação está aplicada para proteger as rotas
    public class PersonController : ControllerBase
    {
        private readonly PersonContext _context;

        public PersonController(PersonContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> CreatePerson([FromBody] PersonRequest req)
        {
            // Cria uma nova pessoa com base no nome fornecido na requisição
            var person = new PersonModel(req.name);
            await _context.AddAsync(person); // Adiciona a pessoa ao contexto do banco de dados
            await _context.SaveChangesAsync(); // Salva as mudanças no banco de dados
            return Ok(person); // Retorna a pessoa criada
        }

        [HttpGet]
        public async Task<IActionResult> GetPeople()
        {
            // Obtém a lista de todas as pessoas no banco de dados
            var people = await _context.People.ToListAsync();
            return Ok(people); // Retorna a lista de pessoas
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdatePerson(Guid id, [FromBody] PersonRequest req)
        {
            // Busca a pessoa pelo ID fornecido
            var person = await _context.People.FirstOrDefaultAsync(x => x.Id == id);

            if (person == null)
                return NotFound(); // Retorna 404 se a pessoa não for encontrada

            person.ChangeName(req.name); // Atualiza o nome da pessoa
            await _context.SaveChangesAsync(); // Salva as mudanças no banco de dados

            return Ok(person); // Retorna a pessoa atualizada
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeletePerson(Guid id)
        {
            // Busca a pessoa pelo ID fornecido
            var person = await _context.People.FirstOrDefaultAsync(x => x.Id == id);

            if (person == null)
                return NotFound(); // Retorna 404 se a pessoa não for encontrada

            person.SetInactive(); // Marca a pessoa como inativa
            await _context.SaveChangesAsync(); // Salva as mudanças no banco de dados

            return Ok(person); // Retorna a pessoa marcada como inativa
        }
    }
}
