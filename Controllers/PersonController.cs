using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Person.Data;
using Person.Models;

namespace Person.Controllers
{
    [ApiController]
    [Route("person")]
    [Authorize] // Ensure this is applied to protect the routes
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
            var person = new PersonModel(req.name);
            await _context.AddAsync(person);
            await _context.SaveChangesAsync();
            return Ok(person);
        }

        [HttpGet]
        public async Task<IActionResult> GetPeople()
        {
            var people = await _context.People.ToListAsync();
            return Ok(people);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdatePerson(Guid id, [FromBody] PersonRequest req)
        {
            var person = await _context.People.FirstOrDefaultAsync(x => x.Id == id);

            if (person == null)
                return NotFound();

            person.ChangeName(req.name);
            await _context.SaveChangesAsync();

            return Ok(person);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeletePerson(Guid id)
        {
            var person = await _context.People.FirstOrDefaultAsync(x => x.Id == id);

            if (person == null)
                return NotFound();

            person.SetInactive();
            await _context.SaveChangesAsync();

            return Ok(person);
        }
    }
}
