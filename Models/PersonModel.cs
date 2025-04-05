using System.ComponentModel.DataAnnotations;

namespace Person.Models
{
    
    public class PersonModel
    {
        /// <param name="name">O nome da pessoa</param>
        public PersonModel(string name)
        {
            // Gera um ID único do tipo GUID (identificador global único)
            Id = Guid.NewGuid();
            
            // Atribui o nome, mas causa uma exceção se o nome for nulo
            Name = name ?? throw new ArgumentNullException(nameof(name));
        }
        
        [Key] // Indica que esta propriedade é a chave primária no banco de dados
        public Guid Id { get; init; } // "init" significa que o ID só pode ser definido na criação do objeto
        
        
        [Required] // Indica que este campo é obrigatório
        [StringLength(100)] // Define o tamanho máximo do nome como 100 caracteres
        public string Name { get; private set; } // "private set" significa que só métodos desta classe podem alterar o nome
        
        /// <param name="name">O novo nome</param>
        public void ChangeName(string name)
        {
            // Atribui o novo nome, mas causa uma exceção se o nome for nulo
            Name = name ?? throw new ArgumentNullException(nameof(name));
        }
        
        public void SetInactive()
        {
            // Marca a pessoa como "desativada" alterando seu nome
            Name = "desativado";
        }
    }
}