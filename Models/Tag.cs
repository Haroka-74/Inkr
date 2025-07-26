using System.ComponentModel.DataAnnotations;

namespace Inkr.Models
{
    public class Tag
    {
        [Key]
        public int Id { get; set; }

        [Required, StringLength(50)]
        public string Name { get; set; } = string.Empty;

        [Required, StringLength(50)]
        public string Slug { get; set; } = string.Empty;

        public ICollection<ArticleTag> ArticleTags { get; set; } = [];
    }
}