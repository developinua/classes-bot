using Core.BotRequests;
using Core.ValueObjects;
using MediatR;
using ResultNet;

namespace Features.Start;

public class StartRequest : BotMessageRequest, IRequest<Result>
{
    protected override string Name => "/start";
    public required long ChatId { get; set; }
    public required Username Username { get; set; }
}