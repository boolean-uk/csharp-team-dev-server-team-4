using Microsoft.EntityFrameworkCore;

namespace exercise.wwwapi.Data;

public sealed class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
        Database.EnsureCreated();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        var seeder = new ModelSeeder();
        seeder.Seed(modelBuilder);
    }
}