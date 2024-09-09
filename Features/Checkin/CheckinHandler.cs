using Features.Interfaces;
using Features.Subscriptions;
using Features.Subscriptions.Services;
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
        await botService.UseChat(request.ChatId, cancel);
        await userSubscriptionService.ShowInformation(request.Username, cancel);
        
        return Result.Success();
    }
}