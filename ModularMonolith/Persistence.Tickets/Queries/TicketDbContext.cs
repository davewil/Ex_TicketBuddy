using Domain.Tickets.ReadModels;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Tickets.Queries;

public class ReadOnlyTicketDbContext(DbContextOptions<ReadOnlyTicketDbContext> options) : DbContext(options)
{
    private const string DefaultSchema = "Ticket";
    public DbSet<Ticket> Tickets => Set<Ticket>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Ticket>().HasKey(t => t.Id);
        modelBuilder.Entity<Ticket>().Property(t => t.SeatNumber).HasConversion(seat => (int)seat, seat => (uint)seat);
        modelBuilder.Entity<Ticket>().ToTable("Tickets", DefaultSchema, t => t.ExcludeFromMigrations());
    }
}
