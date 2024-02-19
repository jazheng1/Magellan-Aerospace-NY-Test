using Microsoft.EntityFrameworkCore;
using MagellanTest.Model;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Item> Items { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure the 'Item' entity to use the 'item' table
            modelBuilder.Entity<Item>().ToTable("item");

            // Additional model configurations can be placed here
        }
}