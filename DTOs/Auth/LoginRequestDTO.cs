using System.ComponentModel.DataAnnotations;

namespace Inkr.DTOs.Auth
{
    public class LoginRequestDTO
    {
        [Required, StringLength(128), EmailAddress]
        public string Email { get; set; } = null!;

        [Required, DataType(DataType.Password)]
        public string Password { get; set; } = null!;
    }
}