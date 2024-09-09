using Features.Interfaces;
using Features.Subscriptions.Services;
using MediatR;
using ResultNet;

namespace Features.Subscriptions.Detailed;

public class SubscriptionsDetailedInformationCallbackHandler(
        IBotService botService,
        IUserSubscriptionService userSubscriptionService,
        IReplyMarkupService replyMarkupService,
        ICallbackExtractorService callbackExtractorService)
    : IRequestHandler<SubscriptionsDetailedInformationCallbackRequest, Result>
{
    public async Task<Result> Handle(
        SubscriptionsDetailedInformationCallbackRequest request,
        CancellationToken cancel)
    {
        await botService.UseChat(request.ChatId, cancel);
        
        var userSubscriptionId = callbackExtractorService.ExtractUserSubscriptionId(
            request.CallbackQuery.Data!, request.CallbackPattern);
        var userSubscription = await userSubscriptionService.GetById(userSubscriptionId);

        if (userSubscription.IsFailure() || userSubscription.Data is null)
        {
            await botService.SendTextMessageAsync(
                "Друже, щось пішло не за планом. " +
                "Я не зміг отримати інформацію про підписку", cancel);
            await botService.SendTextMessageAsync(
                "Якщо проблема повториться, " +
                "напиши @nazikBro і ми усе владнаєм ^^", cancel);
            return Result.Success();
        }

        await botService.SendTextMessageWithReplyAsync(
            userSubscriptionService.ShowDetailedInformation(userSubscription.Data),
            replyMarkupService.GetBackToSubscriptions(userSubscription.Data),
            cancel);
        
        return Result.Success();
    }
}