using Features.Interfaces;
using MediatR;
using ResultNet;

namespace Features.Start;

public class StartHandler(
        IBotService botService,
        IUserService userService)
    : IRequestHandler<StartRequest, Result>
{
    public async Task<Result> Handle(StartRequest request, CancellationToken cancel)
    {
        botService.UseChat(request.ChatId);
        await botService.SendChatActionAsync(cancel);

        if (await userService.UserAlreadyRegistered(request.Username))
        {
            await botService.SendTextMessageAsync("Бот уже стартував :)", cancel);
            return Result.Success();
        }

        var result = await userService.SaveUser(request.Username, request.Message, cancel);
        
        if (result.IsFailure()) return result;

        await botService.SendTextMessageAsync(
            "Натисни /subscriptions щоб переглянути чи купити собі абонемент",
            cancel);
        return Result.Success();
    }
}