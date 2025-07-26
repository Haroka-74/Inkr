using Inkr.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Inkr.Data
{
    public class InkrDbContext(DbContextOptions<InkrDbContext> options) : IdentityDbContext<InkrUser>(options)
    {
        public DbSet<Article> Articles { get; set; } = null!;
        public DbSet<ArticleCategory> ArticleCategories { get; set; } = null!;
        public DbSet<ArticleTag> ArticleTags { get; set; } = null!;
        public DbSet<Category> Categories { get; set; } = null!;
        public DbSet<Comment> Comments { get; set; } = null!;
        public DbSet<RefreshToken> RefreshTokens { get;set; } = null!;
        public DbSet<Tag> Tags { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<RefreshToken>(e =>
            {
                e.HasOne(rt => rt.InkrUser)
                    .WithMany(u => u.RefreshTokens)
                    .HasForeignKey(rt => rt.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<Article>(e =>
            {
                e.HasOne(a => a.Author)
                    .WithMany(u => u.Articles)
                    .HasForeignKey(a => a.AuthorId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<Category>(e =>
            {
                e.HasOne(c => c.Parent)
                    .WithMany(c => c.Children)
                    .HasForeignKey(d => d.ParentId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<ArticleTag>(e =>
            {
                e.HasKey(e => new { e.ArticleId, e.TagId });

                e.HasOne(at => at.Article)
                    .WithMany(a => a.ArticleTags)
                    .HasForeignKey(at => at.ArticleId)
                    .OnDelete(DeleteBehavior.Cascade);

                e.HasOne(at => at.Tag)
                    .WithMany(t => t.ArticleTags)
                    .HasForeignKey(at => at.TagId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<ArticleCategory>(e =>
            {
                e.HasKey(e => new { e.ArticleId, e.CategoryId });

                e.HasOne(ac => ac.Article)
                    .WithMany(a => a.ArticleCategories)
                    .HasForeignKey(ac => ac.ArticleId)
                    .OnDelete(DeleteBehavior.Cascade);

                e.HasOne(ac => ac.Category)
                    .WithMany(c => c.ArticleCategories)
                    .HasForeignKey(ac => ac.CategoryId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<Comment>(e =>
            {
                e.HasOne(c => c.Article)
                    .WithMany(a => a.Comments)
                    .HasForeignKey(c => c.ArticleId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}