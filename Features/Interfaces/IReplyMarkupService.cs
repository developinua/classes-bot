using Core.Aggregates.User;
using Telegram.Bot.Types.ReplyMarkups;

namespace Features.Interfaces;

public interface IReplyMarkupService
{
    IReplyMarkup GetSubscriptions();
    IReplyMarkup GetBackToSubscriptions(UserSubscription userSubscription);
    IReplyMarkup GetSubscriptionsInformation(IReadOnlyCollection<UserSubscription> userSubscriptions);
    IReplyMarkup GetBuySubscription(long subscriptionId);
}