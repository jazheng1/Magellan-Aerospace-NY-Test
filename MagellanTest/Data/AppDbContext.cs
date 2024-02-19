using Microsoft.EntityFrameworkCore;
using MagellanTest.Model;

namespace MagellanTest.Model {
    public class ItemContext : DbContext
    {
        public ItemContext(DbContextOptions<ItemContext> options) : base(options)
        {
        }

        public DbSet<Item> Items { get; set; } = null!;
    }
}