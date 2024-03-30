using System;
using System.Threading.Tasks;
using Domain.Models;
using Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ResultNet;

namespace Data.Repositories;

public interface IUserProfileRepository
{
    Task<Result> CreateAsync(UserProfile userProfile);
    Task<Result<UserProfile?>> GetUserProfileByChatId(long chatId);
    Task<Result> UpdateAsync(UserProfile userProfile);
}

public class UserProfileRepository(
        PostgresDbContext dbContext,
        ILogger<UserProfileRepository> logger)
    : IUserProfileRepository
{
    public async Task<Result<UserProfile?>> GetUserProfileByChatId(long chatId)
    {
        try
        {
            return await dbContext.UserProfiles
                .AsNoTracking()
                .Include(x => x.Culture)
                .FirstOrDefaultAsync(x => x.ChatId == chatId);
        }
        catch(Exception ex)
        {
            logger.LogError(ex, ex.Message);
            return Result.Failure<UserProfile?>().WithMessage("Problem with finding the user.");
        }
    }

    public async Task<Result> CreateAsync(UserProfile userProfile)
    {
        try
        {
            await dbContext.UserProfiles.AddAsync(userProfile);
            await dbContext.SaveChangesAsync();

            return Result.Success();
        }
        catch(Exception ex)
        {
            logger.LogError(ex, ex.Message);
            return Result.Failure().WithMessage("User hasn't been created.");
        }
    }

    public async Task<Result> UpdateAsync(UserProfile userProfile)
    {
        try
        {
            dbContext.UserProfiles.Update(userProfile);
            await dbContext.SaveChangesAsync();

            return Result.Success();
        }
        catch(Exception ex)
        {
            logger.LogError(ex, ex.Message);
            return Result.Failure().WithMessage("User hasn't been updated.");
        }
    }
}