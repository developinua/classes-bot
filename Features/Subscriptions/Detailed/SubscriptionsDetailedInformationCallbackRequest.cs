using Core.BotRequests;
using Core.ValueObjects;
using MediatR;
using ResultNet;
using Telegram.Bot.Types;

namespace Features.Subscriptions.Detailed;

public class SubscriptionsDetailedInformationCallbackRequest : BotCallbackRequest, IRequest<Result>
{
    public override string CallbackPattern => @"(?i)(?<query>user-subscription-id):(?<data>\d+)";
    public long ChatId { get; set; }
    public required Username Username { get; set; }
    public required CallbackQuery CallbackQuery { get; set; }
}