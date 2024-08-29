using Core.Entities;
using Core.Entities.Aggregates.Subscription;
using Core.Entities.Aggregates.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Features.Interfaces;

public interface IClassesDbContext
{
    public DbSet<User> Users { get; }
    public DbSet<Subscription> Subscriptions { get; }
    public DbSet<UserProfile> UserProfiles { get; }
    public DbSet<UserSubscription> UserSubscriptions { get; }
    public DbSet<Culture> Cultures { get; }
    public DbSet<Command> Commands { get; }
    
    public DatabaseFacade Database { get; }
    public Task<int> SaveChangesAsync(CancellationToken cancel);
}