using System.ComponentModel.DataAnnotations;

namespace Inkr.DTOs.Auth
{
    public class RegisterRequestDTO
    {
        [Required, StringLength(50)]
        public string Username { get; set; } = null!;

        [Required, StringLength(128), EmailAddress]
        public string Email { get; set; } = null!;

        [Required, DataType(DataType.Password)]
        public string Password { get; set; } = null!;

        public bool IsAuthor { get; set; } = false;
    }
}