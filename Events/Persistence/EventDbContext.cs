using Domain.Entities;
using Domain.Primitives;
using MassTransit;
using MassTransit.EntityFrameworkCoreIntegration;
using Microsoft.EntityFrameworkCore;
using Event = Domain.Entities.Event;

namespace Persistence;

public class EventDbContext(DbContextOptions<EventDbContext> options) : SagaDbContext(options)
{
    public DbSet<Event> Events => Set<Event>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Event>().HasKey(e => e.Id);
        modelBuilder.Entity<Event>().Property(e => e.Name).HasConversion(name => name.ToString(), name => new Name(name));
        modelBuilder.Entity<Event>().ToTable("Events","Event", e => e.ExcludeFromMigrations());
        modelBuilder.AddInboxStateEntity();
        modelBuilder.AddOutboxMessageEntity();
        modelBuilder.AddOutboxStateEntity();
    }

    protected override IEnumerable<ISagaClassMap> Configurations => null!;
}
