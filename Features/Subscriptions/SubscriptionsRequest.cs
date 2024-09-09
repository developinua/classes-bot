using Core.BotRequests;
using Core.ValueObjects;
using MediatR;
using ResultNet;

namespace Features.Subscriptions;

public class SubscriptionsRequest : BotMessageRequest, IRequest<Result>
{
    protected override string Name => "/підписки";
    public required long ChatId { get; set; }
    public required Username Username { get; set; }
}