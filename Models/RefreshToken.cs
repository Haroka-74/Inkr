using System.ComponentModel.DataAnnotations;

namespace Inkr.Models
{
    public class RefreshToken
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Token { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime ExpiresAt { get; set; }

        public bool IsRevoked { get; set; }

        [Required]
        public string UserId { get; set; } = string.Empty;

        public InkrUser InkrUser { get; set; } = null!;
    }
}