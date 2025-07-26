using System.ComponentModel.DataAnnotations;

namespace Inkr.DTOs.Auth
{
    public class ForgotPasswordDTO
    {
        [Required, StringLength(128), EmailAddress]
        public string Email { get; set; } = null!;
    }
}