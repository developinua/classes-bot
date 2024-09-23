using Core.BotRequests;
using Core.ValueObjects;
using MediatR;
using ResultNet;
using Telegram.Bot.Types;

namespace Features.Subscriptions.Information;

public class SubscriptionsInformationCallbackRequest : BotCallbackRequest, IRequest<Result>
{
    public override string CallbackPattern => "(?i)(?<query>subscription):(?<data>class|course)";
    public long ChatId { get; set; }
    public required Username Username { get; set; }
    public required CallbackQuery CallbackQuery { get; set; }
}