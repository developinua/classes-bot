using Core.Aggregates.Subscription;
using Core.Aggregates.User;
using Core.Keyboard;
using Features.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ResultNet;
using Telegram.Bot.Types.ReplyMarkups;

namespace Features.Subscriptions.Services;

public class UserSubscriptionService(
    IBotService botService,
    IClassesDbContext context,
    ILogger<IUserSubscriptionService> logger)
    : IUserSubscriptionService
{
    public async Task<Result<UserSubscription?>> GetById(long id)
    {
        var response = await context.UserSubscriptions
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id);
        return response;
    }

    public async Task<Result<IReadOnlyCollection<UserSubscription>>> GetByUsername(string username)
    {
        var response = await context.UserSubscriptions
            .Where(x => x.User.NickName == username)
            .ToListAsync();
        return response;
    }

    public async Task<Result<IReadOnlyCollection<UserSubscription>>> GetByUsernameAndType(
        string username, SubscriptionType subscriptionType)
    {
        var response = await context.UserSubscriptions
            .Include(x => x.Subscription)
            .Include(x => x.User)
            .Where(x =>
                x.User.NickName == username
                && x.Subscription.Type == subscriptionType)
            .ToListAsync();
        return response;
    }

    public async Task ShowInformation(string username, CancellationToken cancel)
    {
        var userSubscriptions = await context.UserSubscriptions
            .Include(x => x.Subscription)
            .Where(x => x.User.NickName != null && x.User.NickName.Equals(username))
            .ToListAsync(cancel);

        if (!userSubscriptions.Any())
        {
            await botService.SendTextMessageWithReplyAsync(
                "You have no subscriptions\\. \n Press /підписки to buy one\\.",
                new ReplyKeyboardRemove(),
                cancel);
            return;
        }

        await SendSubscriptionTitle();

        foreach (var userSubscription in userSubscriptions)
            await SendSubscriptionDetails(userSubscription);

        await SendSubscriptionFooter();

        return;

        async Task SendSubscriptionTitle()
        {
            var replyMessage = userSubscriptions.Count == 1 ? "*Your subscription:*" : "*Your subscriptions:*";
            await botService.SendTextMessageWithReplyAsync(replyMessage, new ReplyKeyboardRemove(), cancel);
        }

        async Task SendSubscriptionDetails(UserSubscription userSubscription)
        {
            var replyText =
                "*Subscription:\n*" +
                $"Name: {userSubscription.Subscription.Name}\n" +
                $"SubscriptionType: {userSubscription.Subscription.Type}\n" +
                $"Remaining Classes: {userSubscription.RemainingClasses}\n";
            var replyMarkup = InlineKeyboardBuilder.Create()
                .AddButton("Check-in", $"check-in-subscription-id:{userSubscription.Id}")
                .Build();

            await botService.SendTextMessageWithReplyAsync(replyText, replyMarkup, cancel);
        }

        async Task SendSubscriptionFooter()
        {
            if (!userSubscriptions.Any()) return;

            await botService.SendTextMessageAsync(
                "*Press checkin button on the subscription where you want the class to be taken from*",
                cancel);
        }
    }

    public string ShowDetailedInformation(UserSubscription userSubscription)
    {
        var replyText =
            "*Твоя підписка:\n*" +
            $"Назва: {userSubscription.Subscription.Name}\n" +
            $"Тип: {userSubscription.Subscription.Type}\n" +
            $"Опис: {userSubscription.Subscription.Description}\n" +
            $"Залишилось: {userSubscription.RemainingClasses}\n";
        return replyText;
    }

    public async Task<Result> CheckinOnClass(UserSubscription userSubscription)
    {
        var checkinSuccess = userSubscription.CheckInOnClass();

        if (!checkinSuccess)
        {
            logger.LogError(
                "Remaining classes: {RemainingClassesCount} where not updated for subscription: {SubscriptionId}",
                userSubscription.RemainingClasses, userSubscription.Id);
            Result.Failure().WithMessage("Remaining classes where not updated. Checkin failed.");
        }

        var affectedRows = await context.UserSubscriptions
            .Where(x => x.Id == userSubscription.Id)
            .ExecuteUpdateAsync(updates => updates.SetProperty(p =>
                p.RemainingClasses, userSubscription.RemainingClasses));

        if (affectedRows == 0)
        {
            logger.LogError(
                "Remaining classes: {RemainingClassesCount} where not updated for subscription: {SubscriptionId}",
                userSubscription.RemainingClasses, userSubscription.Id);
            Result.Failure().WithMessage("Remaining classes where not updated. Checkin failed.");
        }

        return Result.Success();
    }
}