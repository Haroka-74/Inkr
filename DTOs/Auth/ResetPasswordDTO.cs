using System.ComponentModel.DataAnnotations;

namespace Inkr.DTOs.Auth
{
    public class ResetPasswordDTO
    {
        [Required]
        public string UserId { get; set; } = null!;

        [Required]
        public string Token { get; set; } = null!;

        [Required, DataType(DataType.Password)]
        public string NewPassword { get; set; } = null!;
    }
}