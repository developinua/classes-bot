using Domain.Requests.Bot;
using MediatR;
using ResultNet;
using Telegram.Bot.Types;

namespace Domain.Requests;

public class CheckinCallbackRequest : BotCallbackRequest, IRequest<Result>
{
    public override string CallbackPattern => @"(?i)(?<query>check-in-user-subscription-id):(?<data>\d+)";
    public long ChatId { get; set; }
    public CallbackQuery CallbackQuery { get; set; } = null!;
}