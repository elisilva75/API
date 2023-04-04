using API.Models;
using Microsoft.EntityFrameworkCore;

public class GroceryContext : DbContext
{
    public GroceryContext(DbContextOptions<GroceryContext> options)
        : base(options)
    {
    }

    public DbSet<Grocery> Groceries { get; set; }
}
