using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Karakatsiya.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Karakatsiya.Models.PageSettings;

namespace Karakatsiya.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Item> Items { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Sale> Sales { get; set; }
        public DbSet<ContactInfo> ContactInfos { get; set; }
        public DbSet<NewsArticle> NewsArticles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Sale>()
                .HasOne(s => s.Item)
                .WithMany(i => i.Sales)
                .HasForeignKey(s => s.ItemId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}