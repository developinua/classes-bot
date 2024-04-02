using System.Threading;
using System.Threading.Tasks;
using Application.Services;
using Domain.Requests;
using MediatR;
using ResultNet;

namespace Application.Handlers.Checkin;

public class CheckinHandler(
        IBotService botService,
        IUserSubscriptionService userSubscriptionService)
    : IRequestHandler<CheckinRequest, Result>
{
    public async Task<Result> Handle(CheckinRequest request, CancellationToken cancellationToken)
    {
        botService.UseChat(request.ChatId);
        await botService.SendChatActionAsync(cancellationToken);
        await userSubscriptionService.ShowUserSubscriptionsInformation(request.Username, cancellationToken);
        
        return Result.Success();
    }
}