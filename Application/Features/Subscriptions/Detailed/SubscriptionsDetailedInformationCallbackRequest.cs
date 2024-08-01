using Core.BotRequests;
using MediatR;
using ResultNet;
using Telegram.Bot.Types;

namespace Application.Features.Subscriptions.Detailed;

public class SubscriptionsDetailedInformationCallbackRequest : BotCallbackRequest, IRequest<Result>
{
    public override string CallbackPattern => @"(?i)(?<query>botUser-subscription-id):(?<data>\d+)";
    public long ChatId { get; set; }
    public string Username { get; set; } = null!;
    public CallbackQuery CallbackQuery { get; set; } = null!;
}