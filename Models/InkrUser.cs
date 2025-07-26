using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Inkr.Models
{
    public class InkrUser : IdentityUser
    {
        [StringLength(255)]
        public string? ProfilePicture { get; set; }

        public string? Bio { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<Article> Articles { get; set; } = [];
        public ICollection<RefreshToken> RefreshTokens { get; set; } = [];
    }
}