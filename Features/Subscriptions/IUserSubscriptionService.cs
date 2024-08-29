using Core.Entities.Aggregates.Subscription;
using Core.Entities.Aggregates.User;
using ResultNet;

namespace Features.Subscriptions;

public interface IUserSubscriptionService
{
    Task<Result<UserSubscription?>> GetById(long id);
    string GetUserSubscriptionInformation(UserSubscription userSubscription);
    Task<Result<IReadOnlyCollection<UserSubscription>>> GetUserSubscriptions(string username);
    Task<Result<IReadOnlyCollection<UserSubscription>>> GetUserSubscriptionsByType(
        string username, SubscriptionType subscriptionType);

    Task ShowUserSubscriptionsInformation(string username, CancellationToken cancel);
    Task<Result> CheckinOnClass(UserSubscription userSubscription);
}