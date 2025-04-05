using System.ComponentModel.DataAnnotations;

namespace Person.Models
{
    public record PersonRequest(
        // Este campo é obrigatório, e mostrará a mensagem de erro especificada se estiver vazio
        [Required(ErrorMessage = "Name is required")]
        // O nome deve ter entre 2 e 100 caracteres, com mensagem de erro personalizada
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Name must be between 2 and 100 characters")]
        string Name);
}