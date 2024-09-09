using Core.BotRequests;
using Core.ValueObjects;
using MediatR;
using ResultNet;
using Telegram.Bot.Types;

namespace Features.Checkin;

public class CheckinRequest : BotMessageRequest, IRequest<Result>
{
    protected override string Name => "/checkin";
    public long ChatId { get; set; }
    public required Username Username { get; set; }
    public required Message Message { get; set; }
}