using Domain.Requests.Bot;
using MediatR;
using ResultNet;
using Telegram.Bot.Types;

namespace Domain.Requests;

public class CheckinRequest : BotMessageRequest, IRequest<Result>
{
    protected override string Name => "/checkin";
    public long ChatId { get; set; }
    public string Username { get; set; } = null!;
    public Message Message { get; set; } = null!;
}