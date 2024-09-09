using Core.Aggregates.Subscription;
using Core.Aggregates.User;
using ResultNet;

namespace Features.Subscriptions.Services;

public interface IUserSubscriptionService
{
    Task<Result<UserSubscription?>> GetById(long id);
    Task<Result<IReadOnlyCollection<UserSubscription>>> GetByUsernameAndType(
        string username,
        SubscriptionType subscriptionType);
    Task<Result<IReadOnlyCollection<UserSubscription>>> GetByUsername(string username);
    Task ShowInformation(string username, CancellationToken cancel);
    string ShowDetailedInformation(UserSubscription userSubscription);
    // todo: extract to checkin
    Task<Result> CheckinOnClass(UserSubscription userSubscription);
}