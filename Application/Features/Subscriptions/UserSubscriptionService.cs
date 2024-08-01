using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Interfaces;
using Core.Aggregates.Subscription;
using Core.Aggregates.User;
using Core.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using ResultNet;
using Telegram.Bot.Types.ReplyMarkups;

namespace Application.Features.Subscriptions;

public class UserSubscriptionService(
    IBotService botService,
    IClassesDbContext context,
    IStringLocalizer<SubscriptionsHandler> localizer,
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

    public string GetUserSubscriptionInformation(UserSubscription userSubscription)
    {
        // todo: add to localization
        var subscription = userSubscription.Subscription;
        var priceText = subscription.DiscountPercent == 0
            ? localizer["SubscriptionPrice", subscription.Price]
            : localizer["SubscriptionPriceWithDiscount", subscription.GetPriceWithDiscount()];
        var subscriptionInformation =
            localizer["SubscriptionName", subscription.Name] +
            priceText +
            localizer["SubscriptionDescription", subscription.Description ?? string.Empty] +
            localizer["SubscriptionType", subscription.Type] +
            localizer["SubscriptionTotalClasses", subscription.ClassesCount] +
            localizer["SubscriptionRemainingClasses", userSubscription.RemainingClasses] +
            localizer["BackToSubscriptions"];

        return subscriptionInformation;
    }

    public async Task<Result<IReadOnlyCollection<UserSubscription>>> GetUserSubscriptions(string username)
    {
        var response = await context.UserSubscriptions
            .Where(x => x.BotUser.NickName == username)
            .ToListAsync();
        return response;
    }

    public async Task<Result<IReadOnlyCollection<UserSubscription>>> GetUserSubscriptionsByType(
        string username, SubscriptionType subscriptionType)
    {
        var response = await context.UserSubscriptions
            .Include(x => x.Subscription)
            .Include(x => x.BotUser)
            .Where(x =>
                x.BotUser.NickName == username
                && x.Subscription.Type == subscriptionType)
            .ToListAsync();
        return response;
    }

    public async Task ShowUserSubscriptionsInformation(string username, CancellationToken cancel)
    {
        var userSubscriptions = await context.UserSubscriptions
            .Include(x => x.Subscription)
            .Where(x => x.BotUser.NickName.Equals(username))
            .ToListAsync(cancel);

        if (!userSubscriptions.Any())
        {
            // todo: localize
            await botService.SendTextMessageWithReplyAsync(
                "You have no subscriptions\\. \n Press /subscriptions to buy one\\.",
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
            // todo: localize
            var replyMessage = userSubscriptions.Count == 1 ? "*Your subscription:*" : "*Your subscriptions:*";
            await botService.SendTextMessageWithReplyAsync(replyMessage, new ReplyKeyboardRemove(), cancel);
        }

        async Task SendSubscriptionDetails(UserSubscription userSubscription)
        {
            // todo: localize
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

            // todo: localize
            await botService.SendTextMessageAsync(
                "*Press checkin button on the subscription where you want the class to be taken from*",
                cancel);
        }
    }

    public Result<bool> CanCheckinOnClass(UserSubscription userSubscription) =>
        userSubscription.CanCheckInOnClass
            ? Result.Failure<bool>().WithMessage("No available classes.")
            : Result.Success(true);

    public async Task<Result> CheckinOnClass(UserSubscription userSubscription)
    {
        userSubscription.CheckInOnClass();
        var affectedRows = await context.UserSubscriptions
            .Where(x => x.Id == userSubscription.Id)
            .ExecuteUpdateAsync(updates => updates.SetProperty(p =>
                p.RemainingClasses, userSubscription.RemainingClasses));

        if (affectedRows == 0)
        {
            logger.LogError(
                "Remaining classes: {RemainingClassesCount} where not updated for subscription: {SubscriptionId}",
                userSubscription.RemainingClasses, userSubscription.Id);
            Result.Failure().WithMessage("Remaining classes where not updated");
        }

        return Result.Success();
    }
}