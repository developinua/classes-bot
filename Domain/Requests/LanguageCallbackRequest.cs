using Domain.Requests.Bot;
using MediatR;
using ResultNet;
using Telegram.Bot.Types;

namespace Domain.Requests;

public class LanguageCallbackRequest : BotCallbackRequest, IRequest<Result>
{
    public override string CallbackPattern => @"(?i)(?<query>language):(?<data>\w{2}-\w{2})";
    public long ChatId { get; set; }
    public CallbackQuery CallbackQuery { get; set; } = null!;
}