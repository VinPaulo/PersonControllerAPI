using System.ComponentModel.DataAnnotations;

namespace Person.Models
{
    /// Representa as credenciais de um usuário
    public class UserCredentials
    {
        // Indica que este campo é obrigatório
        [Required(ErrorMessage = "Username is required")]
        // Define que o nome de usuário deve ter entre 3 e 50 caracteres
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Username must be between 3 and 50 characters")]
        public string Username { get; set; } = string.Empty;
        
        
        // Indica que este campo é obrigatório
        [Required(ErrorMessage = "Password is required")]
        // Define que a senha deve ter entre 6 e 100 caracteres
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be between 6 and 100 characters")]
        public string Password { get; set; } = string.Empty;
    }
}