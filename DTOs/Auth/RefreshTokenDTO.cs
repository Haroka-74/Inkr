using System.ComponentModel.DataAnnotations;

namespace Inkr.DTOs.Auth
{
    public class RefreshTokenDTO
    {
        [Required]
        public string Token { get; set; } = null!;
    }
}