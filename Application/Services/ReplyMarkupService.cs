using System.Collections.Generic;
using Core.Aggregates.Subscription;
using Core.Aggregates.User;
using Core.Keyboard;
using Features.Interfaces;
using Telegram.Bot.Types.ReplyMarkups;

namespace Application.Services;

public class ReplyMarkupService : IReplyMarkupService
{
    public IReplyMarkup GetSubscriptions()
    {
        var replyKeyboardMarkup = InlineKeyboardBuilder.Create()
            .AddButton("Класи", "subscription:class").NewLine()
            .AddButton("Курси", "subscription:course");
        return replyKeyboardMarkup.Build();
    }

    public IReplyMarkup GetBackToSubscriptions(UserSubscription userSubscription)
    {
        var type = userSubscription.Subscription.Type == SubscriptionType.Class ? "class" : "course";
        var replyKeyboardMarkup = InlineKeyboardBuilder.Create()
            .AddButton("Назад", $"subscription:{type}");

        return replyKeyboardMarkup.Build();
    }

    public IReplyMarkup GetSubscriptionsInformation(IReadOnlyCollection<UserSubscription> userSubscriptions)
    {
        var replyKeyboardMarkup = InlineKeyboardBuilder.Create();

        foreach (var userSubscription in userSubscriptions)
        {
            var name = $"Підписка: {userSubscription.Subscription.Name} " +
                       $"(Залишилось: {userSubscription.RemainingClasses})";
            replyKeyboardMarkup.AddButton(name, $"user-subscription-id:{userSubscription.Id}").NewLine();
        }

        return replyKeyboardMarkup.Build();
    }

    public IReplyMarkup GetBuySubscription(long subscriptionId)
    {
        // todo: replacement link
        return InlineKeyboardBuilder.Create()
            .AddUrlButton("Купити", $"paymentlink:{subscriptionId}", "https://google.com")
            .Build();
    }
}