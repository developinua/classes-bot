using Core.Aggregates.User;
using Core.Entities;
using Features.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ResultNet;
using Telegram.Bot.Types;

namespace Features.Start;

public class UserService(
    IUserProfileService userProfileService,
    IClassesDbContext context,
    ILogger<IUserService> logger)
    : IUserService
{
    public async Task<Result> SaveUser(string username, Message message, Culture culture, CancellationToken cancel)
    {
        var user = await GetByUsername(username);
        // todo: use mapster
        var userProfile = new UserProfile
        {
            ChatId = message.From!.Id,
            FirstName = message.From.FirstName,
            LastName = message.From.LastName,
            IsPremium = message.From.IsPremium.GetValueOrDefault(),
            IsBot = message.From.IsBot,
            Culture = culture
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

    public async Task<Result<BotUser?>> GetByUsername(string? username)
    {
        if (string.IsNullOrEmpty(username))
            return Result.Failure<BotUser?>().WithMessage("Username must be filled in");

        var response = await context.Users
            .AsNoTracking()
            .Include(x => x.UserProfile.Culture)
            .FirstOrDefaultAsync(x => x.NickName == username);

        return response;
    }

    public async Task<bool> UserAlreadyRegistered(string username) =>
        await context.Users.AnyAsync(x => x.NickName == username);

    private async Task CreateUser(UserProfile userProfile, string username, CancellationToken cancel)
    {
        await using var transaction = await context.Database.BeginTransactionAsync(cancel);
        try
        {
            var profile = context.UserProfiles.Add(userProfile);
            await context.SaveChangesAsync(cancel);

            context.Users.Add(new BotUser
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
            logger.LogCritical(ex, "Failed to create botUser: {Username}.", username);
        }
    }
}