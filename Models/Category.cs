using System.ComponentModel.DataAnnotations;

namespace Inkr.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required, StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required, StringLength(100)]
        public string Slug { get; set; } = string.Empty;

        public string? Description { get; set; }

        public int? ParentId { get; set; }

        public Category? Parent { get; set; }
        public ICollection<Category> Children { get; set; } = [];
        public ICollection<ArticleCategory> ArticleCategories { get; set; } = [];
    }
}