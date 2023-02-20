using FlowerSpot.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FlowerSpot.Infrastructure.Persistence;
public class FlowerSpotContext : DbContext
{
    public FlowerSpotContext(DbContextOptions<FlowerSpotContext> options) : base(options) { }

    public FlowerSpotContext() { }

    public virtual DbSet<Flower> Flowers { get; set; }
    public virtual DbSet<User> Users { get; set; }
    public virtual DbSet<Sighting> Sightings { get; set; }
    public virtual DbSet<UserSightingLike> UserSightingLikes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(user => user.HasIndex(e => e.Username).IsUnique());
        modelBuilder.Entity<User>().HasIndex(user => user.Username);
        modelBuilder.Entity<Flower>().HasIndex(flower => flower.DateModified);
        modelBuilder.Entity<UserSightingLike>().HasKey(usl => new { usl.UserId, usl.SightingId });
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        foreach (var entry in ChangeTracker.Entries<Flower>())
        {
            entry.Entity.DateModified = DateTime.UtcNow;
        }

        return base.SaveChangesAsync(cancellationToken);
    }
}
