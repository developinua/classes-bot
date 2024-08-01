using Core.BotRequests;
using MediatR;
using ResultNet;

namespace Application.Features.Subscriptions;

public class SubscriptionsRequest : BotMessageRequest, IRequest<Result>
{
    protected override string Name => "/subscriptions";
    public long ChatId { get; set; }
    public string Username { get; set; } = null!;
}