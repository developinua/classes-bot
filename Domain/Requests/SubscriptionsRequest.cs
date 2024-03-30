using Domain.Requests.Bot;
using MediatR;
using ResultNet;

namespace Domain.Requests;

public class SubscriptionsRequest : BotMessageRequest, IRequest<Result>
{
    protected override string Name => "/subscriptions";
    public long ChatId { get; set; }
    public string Username { get; set; } = null!;
}