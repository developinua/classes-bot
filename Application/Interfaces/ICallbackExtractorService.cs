using Core.Aggregates.Subscription;

namespace Application.Interfaces;

public interface ICallbackExtractorService
{
    string GetCultureNameFrom(string? callbackData, string callbackPattern);
    long GetUserSubscriptionId(string callbackData, string callbackPattern);
    SubscriptionType GetSubscriptionType(string callbackData, string callbackPattern);
}