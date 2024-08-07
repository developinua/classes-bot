using Core.BotRequests;
using MediatR;
using ResultNet;
using Telegram.Bot.Types;

namespace Features.Subscriptions.Information;

public class SubscriptionsInformationCallbackRequest : BotCallbackRequest, IRequest<Result>
{
    public override string CallbackPattern => "(?i)(?<query>subscription):(?<data>class|course)";
    public long ChatId { get; set; }
    public string Username { get; set; } = null!;
    public CallbackQuery CallbackQuery { get; set; } = null!;
}