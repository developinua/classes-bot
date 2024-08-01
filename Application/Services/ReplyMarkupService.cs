using System.Collections.Generic;
using Core.Aggregates.Subscription;
using Core.Aggregates.User;
using Core.Utils;
using Features.Interfaces;
using Microsoft.Extensions.Localization;
using Telegram.Bot.Types.ReplyMarkups;

namespace Application.Services;

// public class ReplyMarkupService(IStringLocalizer<SubscriptionsHandler> localizer) : IReplyMarkupService
public class ReplyMarkupService(IStringLocalizer<ReplyMarkupService> localizer) : IReplyMarkupService
{
    public IReplyMarkup GetStartMarkup()
    {
        var replyKeyboardMarkup = InlineKeyboardBuilder.Create()
            .AddButton("English", "language:en-US")
            .AddButton("Українська", "language:uk-UA");
        return replyKeyboardMarkup.Build();
    }

    public IReplyMarkup GetSubscriptions()
    {
        var replyKeyboardMarkup = InlineKeyboardBuilder.Create()
            .AddButton(localizer["SubscriptionClasses"], "subscription:class").NewLine()
            .AddButton(localizer["SubscriptionCourses"], "subscription:course");
        return replyKeyboardMarkup.Build();
    }

    public IReplyMarkup GetBackToSubscriptions(UserSubscription userSubscription)
    {
        var type = userSubscription.Subscription.Type == SubscriptionType.Class ? "class" : "course";
        var replyKeyboardMarkup = InlineKeyboardBuilder.Create()
            .AddButton(localizer["BackToSubscriptions"], $"subscription:{type}");

        return replyKeyboardMarkup.Build();
    }

    public IReplyMarkup GetSubscriptionsInformation(IReadOnlyCollection<UserSubscription> userSubscriptions)
    {
        var replyKeyboardMarkup = InlineKeyboardBuilder.Create();

        foreach (var userSubscription in userSubscriptions)
        {
            // todo: get localized name from the Localization db
            var name = $"Name: {userSubscription.Subscription.Name} (Remaining: {userSubscription.RemainingClasses})";
            replyKeyboardMarkup.AddButton(name, $"botUser-subscription-id:{userSubscription.Id}").NewLine();
        }

        return replyKeyboardMarkup.Build();
    }

    public IReplyMarkup GetBuySubscription(long subscriptionId)
    {
        // todo: replacement link
        return InlineKeyboardBuilder.Create()
            .AddUrlButton("Buy", $"paymentlink:{subscriptionId}", "")
            .Build();
    }
}