using Application.Common.Requests;
using MediatR;
using ResultNet;
using Telegram.Bot.Types;

namespace Application.Features.Start;

public class StartRequest : BotMessageRequest, IRequest<Result>
{
    protected override string Name => "/start";
    public long ChatId { get; set; }
    public string Username { get; set; } = null!;
    public Message Message { get; set; } = null!;
}