using Features.Interfaces;
using Features.Subscriptions.Services;
using MediatR;
using ResultNet;

namespace Features.Subscriptions.Information;

public class SubscriptionsInformationCallbackHandler(
        IBotService botService,
        IReplyMarkupService replyMarkupService,
        ICallbackExtractorService callbackExtractorService,
        IUserSubscriptionService userSubscriptionService)
    : IRequestHandler<SubscriptionsInformationCallbackRequest, Result>
{
    public async Task<Result> Handle(
        SubscriptionsInformationCallbackRequest request,
        CancellationToken cancel)
    {
        await botService.UseChat(request.ChatId, cancel);

        var subscriptionType = callbackExtractorService.ExtractSubscriptionType(
            request.CallbackQuery.Data!, request.CallbackPattern);
        var userSubscriptions = await userSubscriptionService.GetByUsernameAndType(
            request.Username, subscriptionType);

        if (userSubscriptions.IsFailure() || userSubscriptions.Data.Count == 0)
        {
            await botService.SendTextMessageAsync("Немає активних підписок такого типу", cancel);
            await botService.SendTextMessageAsync("Тому це прекрасний шанс, щоб /купити новеньку", cancel);
            return Result.Success();
        }

        await botService.SendTextMessageWithReplyAsync(
            userSubscriptions.Data.Count == 1 ? "Твоя підписка" : "Твої підписки",
            replyMarkupService.GetSubscriptionsInformation(userSubscriptions.Data),
            cancel);
        
        return Result.Success();
    }
}