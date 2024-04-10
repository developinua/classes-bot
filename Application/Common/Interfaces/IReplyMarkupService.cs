using System.Collections.Generic;
using Core.Aggregates.User;
using Telegram.Bot.Types.ReplyMarkups;

namespace Application.Common.Interfaces;

public interface IReplyMarkupService
{
    IReplyMarkup GetStartMarkup();
    IReplyMarkup GetSubscriptions();
    IReplyMarkup GetBackToSubscriptions(UserSubscription userSubscription);
    IReplyMarkup GetSubscriptionsInformation(IReadOnlyCollection<UserSubscription> userSubscriptions);
    IReplyMarkup GetBuySubscription(long subscriptionId);
}