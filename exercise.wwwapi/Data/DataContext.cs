using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace exercise.wwwapi.Data;

public sealed class DataContext : DbContext
{
    private readonly Encryption.EncryptionHelper _encryptionHelper;

    public DataContext(DbContextOptions<DataContext> options, Encryption.EncryptionHelper encryptionHelper) : base(options)
    {
        Database.EnsureCreated();
        _encryptionHelper = encryptionHelper;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var encryptConverter = new ValueConverter<string, string>(
            v => _encryptionHelper.Encrypt(v),
            v => _encryptionHelper.Decrypt(v)
        );

        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Models.User>(entity =>
        {
            entity.Property(e => e.FirstName).HasConversion(encryptConverter);
            entity.Property(e => e.LastName).HasConversion(encryptConverter);
            entity.Property(e => e.Email).HasConversion(encryptConverter);
            entity.Property(e => e.Username).HasConversion(encryptConverter);

        });

        var seeder = new ModelSeeder();
        seeder.Seed(modelBuilder);
    }
}