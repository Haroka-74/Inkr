using System.ComponentModel.DataAnnotations;

namespace Inkr.Models
{
    public class ArticleTag
    {
        [Required]
        public int ArticleId { get; set; }

        [Required]
        public int TagId { get; set; }

        public Article Article { get; set; } = null!;
        public Tag Tag { get; set; } = null!;
    }
}