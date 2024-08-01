using Features.Interfaces;
using Features.Subscriptions;
using MediatR;
using ResultNet;

namespace Features.Checkin;

public class CheckinHandler(
        IBotService botService,
        IUserSubscriptionService userSubscriptionService)
    : IRequestHandler<CheckinRequest, Result>
{
    public async Task<Result> Handle(CheckinRequest request, CancellationToken cancel)
    {
        botService.UseChat(request.ChatId);
        await botService.SendChatActionAsync(cancel);
        await userSubscriptionService.ShowUserSubscriptionsInformation(request.Username, cancel);
        
        return Result.Success();
    }
}