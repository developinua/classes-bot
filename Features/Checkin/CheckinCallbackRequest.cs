using Core.BotRequests;
using MediatR;
using ResultNet;
using Telegram.Bot.Types;

namespace Features.Checkin;

public class CheckinCallbackRequest : BotCallbackRequest, IRequest<Result>
{
    public override string CallbackPattern => @"(?i)(?<query>check-in-user-subscription-id):(?<data>\d+)";
    public long ChatId { get; set; }
    public required CallbackQuery CallbackQuery { get; set; }
}