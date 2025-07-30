using Domain.Entities;
using Domain.Primitives;
using MassTransit;
using MassTransit.EntityFrameworkCoreIntegration;
using Microsoft.EntityFrameworkCore;

namespace Persistence;

public class UserDbContext(DbContextOptions<UserDbContext> options) : SagaDbContext(options)
{
    public DbSet<User> Users => Set<User>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().HasKey(e => e.Id);
        modelBuilder.Entity<User>().Property(e => e.FullName).HasConversion(name => name.ToString(), name => new Name(name));
        modelBuilder.Entity<User>().Property(e => e.Email).HasConversion(email => email.ToString(), email => new Email(email));
        modelBuilder.Entity<User>().ToTable("Users","User", e => e.ExcludeFromMigrations());
        modelBuilder.AddInboxStateEntity();
        modelBuilder.AddOutboxMessageEntity();
        modelBuilder.AddOutboxStateEntity();
    }

    protected override IEnumerable<ISagaClassMap> Configurations => null!;
}
