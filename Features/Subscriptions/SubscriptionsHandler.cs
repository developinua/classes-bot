using Features.Interfaces;
using Features.Subscriptions.Services;
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
        await botService.UseChat(request.ChatId, cancel);

        var userSubscriptions = await userSubscriptionService.GetByUsername(request.Username);

        if (userSubscriptions.Data.Count == 0)
        {
            await botService.SendTextMessageAsync("Немає активних підписок", cancel);
            await botService.SendTextMessageAsync("Тому це прекрасний шанс, щоб /купити новеньку", cancel);
            return Result.Success();
        }

        await botService.SendTextMessageWithReplyAsync(
            "Вибери тип підписки",
            replyMarkupService.GetSubscriptions(),
            cancel);

        return Result.Success();
    }
}