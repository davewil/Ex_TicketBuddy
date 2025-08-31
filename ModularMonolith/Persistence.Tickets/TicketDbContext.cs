using Domain.Tickets.Entities;
using Domain.Tickets.Primitives;
using Microsoft.EntityFrameworkCore;
using Event = Domain.Tickets.Entities.Event;

namespace Persistence.Tickets;

public class TicketDbContext(DbContextOptions<TicketDbContext> options) : DbContext(options)
{
    public DbSet<Event> Events => Set<Event>();
    public DbSet<Ticket> Tickets => Set<Ticket>();
    public DbSet<Venue> Venues => Set<Venue>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Event>().HasKey(e => e.Id);
        modelBuilder.Entity<Event>().Property(e => e.EventName).HasConversion(name => name.ToString(), name => new EventName(name));
        modelBuilder.Entity<Event>().ToTable("Events","Ticket", e => e.ExcludeFromMigrations());
        
        modelBuilder.Entity<Ticket>().HasKey(t => t.Id);
        modelBuilder.Entity<Ticket>().ToTable("Tickets","Ticket", t => t.ExcludeFromMigrations());
        
        modelBuilder.Entity<Venue>().HasKey(v => v.Id);
        modelBuilder.Entity<Venue>().ToTable("EventVenues","Ticket", v => v.ExcludeFromMigrations());
    }
}
