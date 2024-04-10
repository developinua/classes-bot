using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.Features.Subscriptions;
using MediatR;
using ResultNet;

namespace Application.Features.Checkin;

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