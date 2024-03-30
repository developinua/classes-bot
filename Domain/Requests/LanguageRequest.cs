using Domain.Requests.Bot;
using MediatR;
using ResultNet;

namespace Domain.Requests;

public class LanguageRequest : BotMessageRequest, IRequest<Result>
{
    protected override string Name => "/language";
    public long ChatId { get; set; }
    public string Username { get; set; } = null!;
}