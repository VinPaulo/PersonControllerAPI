using Microsoft.EntityFrameworkCore;
using Person.Models;

namespace Person.Data
{
    public class PersonContext : DbContext
    {
        // Construtor que recebe as opções de configuração do banco
        public PersonContext(DbContextOptions<PersonContext> options) : base(options)
        {
            // Aqui tem um código para garantir que o arquivo do banco não esteja travado
            var dbPath = options.Extensions.OfType<Microsoft.EntityFrameworkCore.Infrastructure.RelationalOptionsExtension>().FirstOrDefault()?.ConnectionString;
            if (!string.IsNullOrEmpty(dbPath))
            {
                try
                {
                    var filePath = new Uri(dbPath).LocalPath;
                    if (File.Exists(filePath))
                    {
                        File.SetAttributes(filePath, FileAttributes.Normal);
                    }
                }
                catch
                {
                    // Se der algum problema com o caminho, só ignora e segue em frente
                }
            }
        }
        
        // Esses são os conjuntos de dados que vamos acessar no banco
        public DbSet<User> Users { get; set; }  // Tabela de usuários
        public DbSet<PersonModel> People { get; set; }  // Tabela de pessoas

        // Esse método é chamado quando o Entity Framework está criando o modelo do banco
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Chama o método da classe pai primeiro
            base.OnModelCreating(modelBuilder);
            
            // Define que ela vai ser mapeada para uma tabela chamada "Users"
            modelBuilder.Entity<User>().ToTable("Users");
            
            // Define que ela vai ser mapeada para uma tabela chamada "People"
            modelBuilder.Entity<PersonModel>().ToTable("People");
        }
    }
}