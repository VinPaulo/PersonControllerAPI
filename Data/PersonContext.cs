using Microsoft.EntityFrameworkCore;
using Person.Models;

namespace Person.Data
{
    public class PersonContext : DbContext
    {
        public PersonContext(DbContextOptions<PersonContext> options) : base(options)
        {
        }

        public DbSet<PersonModel> People { get; set; }
    }
}
