using System.ComponentModel.DataAnnotations;

namespace Inkr.Models
{
    public class ArticleCategory
    {
        [Required]
        public int ArticleId { get; set; }

        [Required]
        public int CategoryId { get; set; }

        public Article Article { get; set; } = null!;
        public Category Category { get; set; } = null!;
    }
}