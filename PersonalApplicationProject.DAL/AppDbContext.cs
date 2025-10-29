using Microsoft.EntityFrameworkCore;
using PersonalApplicationProject.DAL.Entities;

namespace PersonalApplicationProject.DAL;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
    public DbSet<Event> Events { get; set; }
    public DbSet<Participant> Participants { get; set; }
    public DbSet<Tag> Tags { get; set; }

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

        modelBuilder.Entity<Tag>()
            .HasIndex(t => t.Name)
            .IsUnique();

        modelBuilder.Entity<Event>()
            .HasMany(e => e.Tags)
            .WithMany(t => t.Events)
            .UsingEntity<Dictionary<string, object>>(
                "EventTag",
                j => j.HasOne<Tag>().WithMany().HasForeignKey("TagId"),
                j => j.HasOne<Event>().WithMany().HasForeignKey("EventId"),
                j =>
                {
                    j.HasKey("EventId", "TagId");
                    j.ToTable("EventTag");
                    j.HasIndex("TagId");
                    j.HasIndex("EventId");
                });

        modelBuilder.Entity<Tag>().HasData(
            new Tag { Id = 1, Name = "technology" },
            new Tag { Id = 2, Name = "sports" },
            new Tag { Id = 3, Name = "music" }
        );
    }
}