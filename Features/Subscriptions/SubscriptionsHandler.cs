using Features.Interfaces;
using MediatR;
using ResultNet;

namespace Features.Subscriptions;

public class SubscriptionsHandler(
        IBotService botService,
        IReplyMarkupService replyMarkupService,
        IUserSubscriptionService userSubscriptionService)
    : IRequestHandler<SubscriptionsRequest, Result>
{
    public async Task<Result> Handle(SubscriptionsRequest request, CancellationToken cancel)
    {
        botService.UseChat(request.ChatId);
        await botService.SendChatActionAsync(cancel);

        var userSubscriptions = await userSubscriptionService.GetUserSubscriptions(request.Username);

        if (userSubscriptions.Data.Count == 0)
        {
            await botService.SendTextMessageAsync("NoSubscriptions", cancel);
            return Result.Success();
        }

        await botService.SendTextMessageWithReplyAsync(
            "ChooseSubscriptionType",
            replyMarkupService.GetSubscriptions(),
            cancel);

        return Result.Success();
    }
}