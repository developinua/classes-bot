using Core.BotRequests;
using MediatR;
using ResultNet;
using Telegram.Bot.Types;

namespace Application.Features.Checkin;

public class CheckinCallbackRequest : BotCallbackRequest, IRequest<Result>
{
    public override string CallbackPattern => @"(?i)(?<query>check-in-botUser-subscription-id):(?<data>\d+)";
    public long ChatId { get; set; }
    public CallbackQuery CallbackQuery { get; set; } = null!;
}