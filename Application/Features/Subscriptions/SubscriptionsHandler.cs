using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Localization;
using ResultNet;

namespace Application.Features.Subscriptions;

public class SubscriptionsHandler(
        IBotService botService,
        IReplyMarkupService replyMarkupService,
        IUserSubscriptionService userSubscriptionService,
        IStringLocalizer<SubscriptionsHandler> localizer)
    : IRequestHandler<SubscriptionsRequest, Result>
{
    public async Task<Result> Handle(SubscriptionsRequest request, CancellationToken cancel)
    {
        botService.UseChat(request.ChatId);
        await botService.SendChatActionAsync(cancel);

        var userSubscriptions = await userSubscriptionService.GetUserSubscriptions(request.Username);

        if (userSubscriptions.Data.Count == 0)
        {
            await botService.SendTextMessageAsync(localizer.GetString("NoSubscriptions"), cancel);
            return Result.Success();
        }

        await botService.SendTextMessageWithReplyAsync(
            localizer.GetString("ChooseSubscriptionType"),
            replyMarkupService.GetSubscriptions(),
            cancel);

        return Result.Success();
    }
}