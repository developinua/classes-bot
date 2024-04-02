using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Extensions;
using Domain.Models;
using Domain.Models.Enums;
using Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ResultNet;

namespace Data.Repositories;

public interface IUserSubscriptionRepository
{
    Task<Result> Add(UserSubscription userSubscription);
    Task<Result> AddRange(IReadOnlyCollection<UserSubscription> userSubscriptions);
    Task<Result<UserSubscription?>> GetById(long id);
    Task<Result<IReadOnlyCollection<UserSubscription>>> GetActiveByUsername(string username);
    Task<Result<UserSubscription?>> GetActiveByUsernameAndType(string username, SubscriptionType subscriptionType);
    Task<Result<IReadOnlyCollection<UserSubscription>>> GetUserSubscriptions(string username);

    Task<Result<IReadOnlyCollection<UserSubscription>>> GetUserSubscriptionsByType(
        string username, SubscriptionType subscriptionType);

    Task<Result> ReduceRemainingClasses(UserSubscription userSubscription);
}

public class UserSubscriptionRepository(
    PostgresDbContext dbContext,
    ILogger<UserSubscriptionRepository> logger)
    : IUserSubscriptionRepository
{
    public async Task<Result> Add(UserSubscription userSubscription)
    {
        try
        {
            await dbContext.UserSubscriptions.AddAsync(userSubscription);
            await dbContext.SaveChangesAsync();

            return Result.Success();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            return Result.Failure().WithMessage("User subscription hasn't been created.");
        }
    }

    public async Task<Result> AddRange(IReadOnlyCollection<UserSubscription> userSubscriptions)
    {
        try
        {
            await dbContext.UserSubscriptions.AddRangeAsync(userSubscriptions);
            await dbContext.SaveChangesAsync();

            return Result.Success();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            return Result.Failure().WithMessage("User subscription hasn't been created.");
        }
    }

    public async Task<Result<UserSubscription?>> GetById(long id)
    {
        try
        {
            var userSubscription = await dbContext.UserSubscriptions
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id.Equals(id) && x.Subscription.IsActive);
            return Result.Success(userSubscription);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            return Result.Failure<UserSubscription?>()
                .WithMessage("Can't get user subscription by id.");
        }
    }

    public async Task<Result<IReadOnlyCollection<UserSubscription>>> GetActiveByUsername(string username)
    {
        try
        {
            var userSubscriptions = await dbContext.UserSubscriptions
                .Include(x => x.Subscription)
                .Where(x =>
                    x.User.NickName.Equals(username)
                    && x.RemainingClasses > 0
                    && x.Subscription.IsActive)
                .ToListAsync();
            return Result.Success(userSubscriptions.AsReadOnlyCollection());
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            return Result.Failure<IReadOnlyCollection<UserSubscription>>()
                .WithMessage("Can't get user subscriptions by username.");
        }
    }

    public async Task<Result<UserSubscription?>> GetActiveByUsernameAndType(
        string username,
        SubscriptionType subscriptionType)
    {
        try
        {
            var response = await dbContext.UserSubscriptions
                .FirstOrDefaultAsync(x =>
                    x.User.NickName == username
                    && x.Subscription.Type == subscriptionType
                    && x.RemainingClasses > 0
                    && x.Subscription.IsActive);
            return Result.Success(response);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            return Result.Failure<UserSubscription?>()
                .WithMessage("Can't get user subscription by username and type.");
        }
    }

    public async Task<Result<IReadOnlyCollection<UserSubscription>>> GetUserSubscriptions(string username)
    {
        try
        {
            var response = await dbContext.UserSubscriptions
                .Where(x =>
                    x.User.NickName == username
                    && x.RemainingClasses > 0
                    && x.Subscription.IsActive)
                .ToListAsync();
            return Result.Success(response.AsReadOnlyCollection());
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            return Result.Failure<Result<IReadOnlyCollection<UserSubscription>>>()
                .WithMessage("Can't get active user subscriptions with classes by username.");
        }
    }

    public async Task<Result<IReadOnlyCollection<UserSubscription>>> GetUserSubscriptionsByType(
        string username,
        SubscriptionType subscriptionType)
    {
        try
        {
            var response = await dbContext.UserSubscriptions
                .Include(x => x.Subscription)
                .Include(x => x.User)
                .Where(x =>
                    x.User.NickName == username
                    && x.Subscription.Type == subscriptionType
                    && x.RemainingClasses > 0
                    && x.Subscription.IsActive)
                .ToListAsync();
            return Result.Success(response.AsReadOnlyCollection());
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            return Result.Failure<Result<IReadOnlyCollection<UserSubscription>>>()
                .WithMessage("Can't get active user subscriptions with classes by username.");
        }
    }

    public async Task<Result> ReduceRemainingClasses(UserSubscription userSubscription)
    {
        try
        {
            var updatedRemainingClasses = userSubscription.RemainingClasses--;
            var affectedRows = await dbContext.UserSubscriptions
                .Where(x => x.Id == userSubscription.Id)
                .ExecuteUpdateAsync(updates => updates.SetProperty(p => p.RemainingClasses, updatedRemainingClasses));

            if (affectedRows == 0)
            {
                logger.LogError("Remaining classes where not updated {RemainingClasses}",
                    JsonConvert.SerializeObject(userSubscription));
                Result.Failure().WithMessage("Remaining classes where not updated");
            }
            
            return Result.Success();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            return Result.Failure().WithMessage("User subscription hasn't been updated.");
        }
    }
}