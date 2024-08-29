using System.Collections.Generic;
using Core.Entities.Aggregates.Subscription;
using Core.Entities.Aggregates.User;
using Core.Keyboard;
using Features.Interfaces;
using Telegram.Bot.Types.ReplyMarkups;

namespace Application.Services;

public class ReplyMarkupService : IReplyMarkupService
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
            .AddButton("SubscriptionClasses", "subscription:class").NewLine()
            .AddButton("SubscriptionCourses", "subscription:course");
        return replyKeyboardMarkup.Build();
    }

    public IReplyMarkup GetBackToSubscriptions(UserSubscription userSubscription)
    {
        var type = userSubscription.Subscription.Type == SubscriptionType.Class ? "class" : "course";
        var replyKeyboardMarkup = InlineKeyboardBuilder.Create()
            .AddButton("BackToSubscriptions", $"subscription:{type}");

        return replyKeyboardMarkup.Build();
    }

    public IReplyMarkup GetSubscriptionsInformation(IReadOnlyCollection<UserSubscription> userSubscriptions)
    {
        var replyKeyboardMarkup = InlineKeyboardBuilder.Create();

        foreach (var userSubscription in userSubscriptions)
        {
            var name = $"Name: {userSubscription.Subscription.Name} (Remaining: {userSubscription.RemainingClasses})";
            replyKeyboardMarkup.AddButton(name, $"user-subscription-id:{userSubscription.Id}").NewLine();
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