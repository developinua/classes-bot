using Core.Aggregates.Subscription;

namespace Features.Interfaces;

public interface ICallbackExtractorService
{
    string ExtractCultureNameFrom(string? callbackData, string callbackPattern);
    long ExtractUserSubscriptionId(string callbackData, string callbackPattern);
    SubscriptionType ExtractSubscriptionType(string callbackData, string callbackPattern);
}