using Microsoft.EntityFrameworkCore;
using PersonalApplicationProject.DAL.Entities;

namespace PersonalApplicationProject.DAL;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
    public DbSet<Event> Events { get; set; }
    public DbSet<Participant> Participants { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Participant>().HasKey(p => new { p.UserId, p.EventId });

        modelBuilder.Entity<Participant>().HasOne(p => p.User).WithMany(u => u.Participations)
            .HasForeignKey(p => p.UserId).OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Participant>().HasOne(p => p.Event).WithMany(e => e.Participants)
            .HasForeignKey(p => p.EventId).OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<User>().HasMany(u => u.OrganizedEvents).WithOne(e => e.Organizer)
            .HasForeignKey(e => e.OrganizerId).OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Event>().HasMany<Tag>().WithMany(t => t.Events).UsingEntity(j => j.ToTable("EventTag"));
    }
}