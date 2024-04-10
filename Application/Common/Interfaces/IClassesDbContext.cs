using System.Threading;
using System.Threading.Tasks;
using Core.Entities;
using Core.Entities.Subscription;
using Core.Entities.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Application.Common.Interfaces;

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