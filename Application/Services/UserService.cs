using System;
using System.Threading.Tasks;
using Data.Context;
using Data.Repositories;
using Domain.Models;
using Domain.Requests;
using Microsoft.Extensions.Logging;
using ResultNet;

namespace Application.Services;

public interface IUserService
{
    Task<Result> SaveUser(StartRequest request, Culture culture);
    Task<Result<User?>> GetByUsername(string? username);
    Task<bool> UserAlreadyRegistered(string username);
}

public class UserService(
        IUserProfileService userProfileService,
        IUserRepository userRepository,
        PostgresDbContext dbContext,
        ILogger<UserService> logger)
    : IUserService
{
    public async Task<Result> SaveUser(StartRequest request, Culture culture)
    {
        var user = await GetByUsername(request.Username);
        var userProfile = userProfileService.GetUserProfileFromMessage(request.Message, culture);

        return user.Data is null
            ? await CreateUser(userProfile, request.Username)
            : await userProfileService.UpdateUserProfile(userProfile);
    }

    public async Task<Result<User?>> GetByUsername(string? username)
    {
        if (string.IsNullOrEmpty(username))
            return Result.Failure<User?>().WithMessage("Username must be filled in");

        return await userRepository.GetByUsername(username);
    }

    public async Task<bool> UserAlreadyRegistered(string username) =>
        await userRepository.UserExists(username);

    private async Task<Result> CreateUser(UserProfile userProfile, string nickname)
    {
        await using var transaction = await dbContext.Database.BeginTransactionAsync();
        try
        {
            var profile = await userProfileService.Create(userProfile);

            if (profile.IsFailure())
            {
                await transaction.RollbackAsync();
                return Result.Failure().WithMessage("Failed to create user profile");
            }
            
            var user = await userRepository.CreateAsync(new User
            {
                NickName = nickname,
                UserProfile = userProfile
            });

            if (user.IsFailure())
            {
                await transaction.RollbackAsync();
                return Result.Failure().WithMessage("Failed to create user");
            }
                    
            await dbContext.SaveChangesAsync();
            await transaction.CommitAsync();

            return Result.Success();
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            logger.LogCritical(ex, "Failed to create user: {Username}.", nickname);

            return Result.Failure().WithMessage("Failed to create user");
        }
    }
}