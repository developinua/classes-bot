using Application.Common.Requests;
using MediatR;
using ResultNet;

namespace Application.Features.Language;

public class LanguageRequest : BotMessageRequest, IRequest<Result>
{
    protected override string Name => "/language";
    public long ChatId { get; set; }
    public string Username { get; set; } = null!;
}