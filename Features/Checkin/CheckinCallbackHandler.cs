using Features.Interfaces;
using Features.Subscriptions;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ResultNet;
using Telegram.Bot.Types;

namespace Features.Checkin;

public class CheckinCallbackHandler(
        IBotService botService,
        IClassesDbContext context,
        IUserSubscriptionService userSubscriptionService,
        ICallbackExtractorService callbackExtractorService,
        IValidator<CallbackQuery> callbackQueryValidator)
    : IRequestHandler<CheckinCallbackRequest, Result>
{
    public async Task<Result> Handle(CheckinCallbackRequest request, CancellationToken cancel)
    {
        if ((await callbackQueryValidator.ValidateAsync(request.CallbackQuery, cancel)).IsValid)
            return Result.Failure().WithMessage("Invalid check-in parameters.");

        botService.UseChat(request.ChatId);
        await botService.SendChatActionAsync(cancel);

        var userSubscriptionId = callbackExtractorService.GetUserSubscriptionId(
            request.CallbackQuery.Data!, request.CallbackPattern);
        var userSubscription = await context.UserSubscriptions
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == userSubscriptionId, cancellationToken: cancel);

        if (userSubscription is null)
        {
            await botService.SendTextMessageAsync("SubscriptionNotFound", cancel);
            return Result.Failure().WithMessage("User subscription not found.");
        }

        if (!userSubscription.CanCheckinOnClass())
        {
            await botService.SendTextMessageAsync("NoAvailableClasses", cancel);
            return Result.Failure().WithMessage("No available classes.");
        }
        
        var checkin = await userSubscriptionService.CheckinOnClass(userSubscription);

        if (checkin.IsFailure())
        {
            await botService.SendTextMessageAsync("ClassCheckinProblem", cancel);
            return Result.Failure().WithMessage("There was a problem with class check-in.");
        }
        
        await botService.SendTextMessageAsync("*💚*", cancel);
        return Result.Success();
    }
}