using ChatApp.Server.Entities.Abstracts;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.Server.Entities.Context;

public class ChatAppContext : DbContext
{
    public ChatAppContext(DbContextOptions<ChatAppContext> options) : base(options)
    {
    }
    
    public DbSet<User> Users { get; set; }
    public DbSet<Message> Messages { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>().HasIndex(x => x.UserName).IsUnique();
        modelBuilder.Entity<User>().HasIndex(x => x.Email).IsUnique();
        
        modelBuilder.Entity<User>().HasMany(x => x.ReceivedMessages).WithOne(x => x.ToUser).OnDelete(DeleteBehavior.ClientSetNull);
        modelBuilder.Entity<User>().HasMany(x => x.SentMessages).WithOne(x => x.FromUser).OnDelete(DeleteBehavior.Cascade);
    }

    public override int SaveChanges()
    {
        var timestampedEntries = ChangeTracker.Entries().Where(x => x.Entity is TimestampTrackedEntity && x.State == EntityState.Added || x.State == EntityState.Modified);

        foreach (var entry in timestampedEntries)
        {
            var entity = entry.Entity as TimestampTrackedEntity;
         
            if (entry.State == EntityState.Added)
            {
                entity.CreatedAt = DateTime.UtcNow;
            }
            
            if (entry.State == EntityState.Modified)
            {
                entity.UpdatedAt = DateTime.UtcNow;
            }
        }
        
        return base.SaveChanges();
    }
}