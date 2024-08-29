using Core.Entities.Aggregates.User;
using Features.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ResultNet;
using Telegram.Bot.Types;
using User = Core.Entities.Aggregates.User.User;

namespace Features.Start;

public class UserService(
    IUserProfileService userProfileService,
    IClassesDbContext context,
    ILogger<IUserService> logger)
    : IUserService
{
    // todo: change to check chatId or username or phone
    public async Task<Result> SaveUser(string username, Message message, CancellationToken cancel)
    {
        var user = await GetByUsername(username);
        // todo: use mapster
        var userProfile = new UserProfile
        {
            ChatId = message.From!.Id,
            FirstName = message.From.FirstName,
            LastName = message.From.LastName
        };

        if (user.Data is null)
        {
            await CreateUser(userProfile, username, cancel);
        }
        else
        {
            await userProfileService.UpdateUserProfile(userProfile, cancel);
        }
        
        return Result.Success();
    }

    public async Task<Result<User?>> GetByUsername(string? username)
    {
        if (string.IsNullOrEmpty(username))
            return Result.Failure<User?>().WithMessage("Username must be filled in");

        var response = await context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.NickName == username);

        return response;
    }

    // todo: change to check chatId or username or phone
    public async Task<bool> UserAlreadyRegistered(string username) =>
        await context.Users.AnyAsync(x => x.NickName == username);

    private async Task CreateUser(UserProfile userProfile, string username, CancellationToken cancel)
    {
        await using var transaction = await context.Database.BeginTransactionAsync(cancel);
        try
        {
            var profile = context.UserProfiles.Add(userProfile);
            await context.SaveChangesAsync(cancel);

            context.Users.Add(new User
            {
                NickName = username,
                UserProfile = profile.Entity
            });
            await context.SaveChangesAsync(cancel);
            await transaction.CommitAsync(cancel);
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync(cancel);
            logger.LogCritical(ex, "Failed to create user: {Username}.", username);
        }
    }
}