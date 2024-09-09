using System.Reflection;
using Core.Aggregates.Subscription;
using Core.Aggregates.User;
using Core.Entities;
using Features.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Db.Context;

public class ClassesPostgresDbContext(DbContextOptions<ClassesPostgresDbContext> options)
    : DbContext(options), IClassesDbContext
{
    public DbSet<User> Users => Set<User>();
    public DbSet<Subscription> Subscriptions => Set<Subscription>();
    public DbSet<UserProfile> UserProfiles => Set<UserProfile>();
    public DbSet<UserSubscription> UserSubscriptions => Set<UserSubscription>();
    public DbSet<Culture> Cultures => Set<Culture>();
    public DbSet<Command> Commands => Set<Command>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}