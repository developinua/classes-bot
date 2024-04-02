using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Handlers.Subscriptions;
using Application.Utils;
using Data.Repositories;
using Domain.Models;
using Domain.Models.Enums;
using Microsoft.Extensions.Localization;
using ResultNet;
using Telegram.Bot.Types.ReplyMarkups;

namespace Application.Services;

public interface IUserSubscriptionService
{
    Task<Result<UserSubscription?>> GetById(long id);
    string GetUserSubscriptionInformation(UserSubscription userSubscription);
    Task<Result<IReadOnlyCollection<UserSubscription>>> GetUserSubscriptions(string username);
    Task<Result<IReadOnlyCollection<UserSubscription>>> GetUserSubscriptionsByType(
        string username, SubscriptionType subscriptionType);

    Task ShowUserSubscriptionsInformation(string username, CancellationToken cancellationToken);
    Result<bool> CanCheckinOnClass(UserSubscription userSubscription);
    Task<Result> CheckinOnClass(UserSubscription userSubscription);
}

public class UserSubscriptionService(
    IBotService botService,
    IUserSubscriptionRepository userSubscriptionRepository,
    IStringLocalizer<SubscriptionsHandler> localizer)
    : IUserSubscriptionService
{
    public async Task<Result<UserSubscription?>> GetById(long id) =>
        await userSubscriptionRepository.GetById(id);

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

    public async Task<Result<IReadOnlyCollection<UserSubscription>>> GetUserSubscriptions(string username) =>
        await userSubscriptionRepository.GetUserSubscriptions(username);

    public async Task<Result<IReadOnlyCollection<UserSubscription>>> GetUserSubscriptionsByType(
        string username, SubscriptionType subscriptionType) =>
        await userSubscriptionRepository.GetUserSubscriptionsByType(username, subscriptionType);

    public async Task ShowUserSubscriptionsInformation(string username, CancellationToken cancellationToken)
    {
        var userSubscriptions = await userSubscriptionRepository.GetActiveByUsername(username);

        if (!userSubscriptions.Data.Any())
        {
            // todo: localize
            await botService.SendTextMessageWithReplyAsync(
                "You have no subscriptions\\. \n Press /subscriptions to buy one\\.",
                replyMarkup: new ReplyKeyboardRemove(),
                cancellationToken: cancellationToken);
            return;
        }

        await SendSubscriptionTitle();

        foreach (var userSubscription in userSubscriptions.Data)
            await SendSubscriptionDetails(userSubscription);

        await SendSubscriptionFooter();

        return;

        async Task SendSubscriptionTitle()
        {
            // todo: localize
            var replyMessage = userSubscriptions.Data.Count == 1 ? "*Your subscription:*" : "*Your subscriptions:*";

            await botService.SendTextMessageWithReplyAsync(
                replyMessage,
                replyMarkup: new ReplyKeyboardRemove(),
                cancellationToken: cancellationToken);
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

            await botService.SendTextMessageWithReplyAsync(replyText, replyMarkup, cancellationToken);
        }

        async Task SendSubscriptionFooter()
        {
            if (!userSubscriptions.Data.Any()) return;

            // todo: localize
            await botService.SendTextMessageAsync(
                "*Press checkin button on the subscription where you want the class to be taken from*",
                cancellationToken);
        }
    }

    public Result<bool> CanCheckinOnClass(UserSubscription userSubscription) =>
        userSubscription.RemainingClasses == 0
            ? Result.Failure<bool>().WithMessage("No available classes.")
            : Result.Success(true);

    public async Task<Result> CheckinOnClass(UserSubscription userSubscription)
    {
        if (userSubscription.RemainingClasses == 0)
            return Result.Failure().WithMessage("No available classes.");

        return await userSubscriptionRepository.ReduceRemainingClasses(userSubscription);
    }
}