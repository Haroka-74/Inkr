using System.ComponentModel.DataAnnotations;

namespace Inkr.Models
{
    public class Comment
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int ArticleId { get; set; }

        [Required, StringLength(100)]
        public string AuthorName { get; set; } = string.Empty;

        [Required, StringLength(100), EmailAddress]
        public string AuthorEmail { get; set; } = string.Empty;

        [Required]
        public string Content { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public Article Article { get; set; } = null!;
    }
}