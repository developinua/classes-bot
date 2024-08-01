using Core.BotRequests;
using MediatR;
using ResultNet;

namespace Features.Language;

public class LanguageRequest : BotMessageRequest, IRequest<Result>
{
    protected override string Name => "/language";
    public long ChatId { get; set; }
    public string Username { get; set; } = null!;
}