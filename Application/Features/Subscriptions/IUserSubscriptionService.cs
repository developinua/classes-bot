using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Core.Aggregates.Subscription;
using Core.Aggregates.User;
using ResultNet;

namespace Application.Features.Subscriptions;

public interface IUserSubscriptionService
{
    Task<Result<UserSubscription?>> GetById(long id);
    string GetUserSubscriptionInformation(UserSubscription userSubscription);
    Task<Result<IReadOnlyCollection<UserSubscription>>> GetUserSubscriptions(string username);
    Task<Result<IReadOnlyCollection<UserSubscription>>> GetUserSubscriptionsByType(
        string username, SubscriptionType subscriptionType);

    Task ShowUserSubscriptionsInformation(string username, CancellationToken cancel);
    Result<bool> CanCheckinOnClass(UserSubscription userSubscription);
    Task<Result> CheckinOnClass(UserSubscription userSubscription);
}