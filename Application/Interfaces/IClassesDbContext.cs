using System.Threading;
using System.Threading.Tasks;
using Core.Aggregates.Subscription;
using Core.Aggregates.User;
using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Application.Interfaces;

public interface IClassesDbContext
{
    public DbSet<BotUser> Users { get; }
    public DbSet<Subscription> Subscriptions { get; }
    public DbSet<UserProfile> UserProfiles { get; }
    public DbSet<UserSubscription> UserSubscriptions { get; }
    public DbSet<Culture> Cultures { get; }
    public DbSet<Command> Commands { get; }
    
    public DatabaseFacade Database { get; }
    public Task<int> SaveChangesAsync(CancellationToken cancel);
}