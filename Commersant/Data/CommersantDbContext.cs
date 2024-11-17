using Commersant.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Commersant.Data
{
    public class CommersantDbContext : DbContext
    {
        public DbSet<Item> Items { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<ItemCategory> ItemCategories { get; set; }

        public CommersantDbContext(DbContextOptions<CommersantDbContext> options)
           : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<ItemCategory>()
                .HasKey(ic => new { ic.ItemId, ic.CategoryId });

            modelBuilder.Entity<ItemCategory>()
                .HasOne(ic => ic.Item)
                .WithMany(i => i.ItemCategories)
                .HasForeignKey(ic => ic.ItemId);

            modelBuilder.Entity<ItemCategory>()
                .HasOne(ic => ic.Category)
                .WithMany(c => c.ItemCategories)
                .HasForeignKey(ic => ic.CategoryId);

            modelBuilder.Entity<ItemCategory>()
                .HasIndex(ic => ic.CategoryId)
                .HasDatabaseName("IX_ItemCategories_CategoryId");

            modelBuilder.Entity<ItemCategory>()
                .HasIndex(ic => ic.ItemId)
                .HasDatabaseName("IX_ItemCategories_ItemId");
        }
       
    }
}
