using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Localization;
using ResultNet;
using Telegram.Bot.Types;

namespace Application.Features.Subscriptions.Information;

public class SubscriptionsInformationCallbackHandler(
        IBotService botService,
        IReplyMarkupService replyMarkupService,
        ICallbackExtractorService callbackExtractorService,
        IUserSubscriptionService userSubscriptionService,
        IStringLocalizer<SubscriptionsHandler> localizer,
        IValidator<CallbackQuery> validator)
    : IRequestHandler<SubscriptionsInformationCallbackRequest, Result>
{
    public async Task<Result> Handle(
        SubscriptionsInformationCallbackRequest request,
        CancellationToken cancel)
    {
        if ((await validator.ValidateAsync(request.CallbackQuery, cancel)).IsValid)
            Result.Failure().WithMessage("No valid callback query.");

        botService.UseChat(request.ChatId);
        await botService.SendChatActionAsync(cancel);

        var subscriptionType = callbackExtractorService.GetSubscriptionType(
            request.CallbackQuery.Data!, request.CallbackPattern);
        var userSubscriptions = await userSubscriptionService.GetUserSubscriptionsByType(
            request.Username, subscriptionType);

        if (userSubscriptions.IsFailure() || userSubscriptions.Data.Count == 0)
        {
            await botService.SendTextMessageAsync(localizer.GetString("NoSubscriptions"), cancel);
            return Result.Success();
        }

        // todo: add delete previous saved detailed information message (if such exists)
        await botService.SendTextMessageWithReplyAsync(
            localizer.GetString("YourSubscriptions"),
            replyMarkupService.GetSubscriptionsInformation(userSubscriptions.Data),
            cancel);
        // todo: add save message when press any of choice button to delete it next
        
        return Result.Success();
    }
}