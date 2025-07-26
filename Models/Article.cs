using Inkr.Enums;
using System.ComponentModel.DataAnnotations;

namespace Inkr.Models
{
    public class Article
    {
        [Key]
        public int Id { get; set; }

        [Required, StringLength(255)]
        public string Title { get; set; } = string.Empty;

        [Required, StringLength(255)]
        public string Slug { get; set; } = string.Empty;

        [Required]
        public string Content { get; set; } = string.Empty;

        public string? Excerpt { get; set; }

        [Required]
        public ArticleStatus Status { get; set; } = ArticleStatus.Draft;

        [Required]
        public string AuthorId { get; set; } = string.Empty;

        [Range(0, int.MaxValue)]
        public int ViewCount { get; set; } = 0;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public InkrUser Author { get; set; } = null!;
        public ICollection<ArticleTag> ArticleTags { get; set; } = [];
        public ICollection<ArticleCategory> ArticleCategories { get; set; } = [];
        public ICollection<Comment> Comments { get; set; } = [];
    }
}