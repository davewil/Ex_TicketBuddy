using Domain.Events.Primitives;
using Microsoft.EntityFrameworkCore;
using Event = Domain.Events.Entities.Event;

namespace Infrastructure.Events.Persistence;

public class EventDbContext(DbContextOptions<EventDbContext> options) : DbContext(options)
{
    public DbSet<Event> Events => Set<Event>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Event>().HasKey(e => e.Id);
        modelBuilder.Entity<Event>().Property(e => e.EventName).HasConversion(name => name.ToString(), name => new EventName(name));
        modelBuilder.Entity<Event>().ToTable("Events","Event", e => e.ExcludeFromMigrations());
    }
}
