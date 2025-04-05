using System;
using System.ComponentModel.DataAnnotations;

namespace Person.Models
{
    public class User
    {
        public User()
        {
        }

        public User(int id, string username, string passwordHash)
        {
            Id = id;
            Username = username ?? throw new ArgumentNullException(nameof(username));
            PasswordHash = passwordHash ?? throw new ArgumentNullException(nameof(passwordHash));
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Username { get; set; } = string.Empty;

        [Required]
        public string PasswordHash { get; set; } = string.Empty;
        
    }
}