using Microsoft.EntityFrameworkCore;
using MagellanTest.Model;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Item> Items { get; set; }
}