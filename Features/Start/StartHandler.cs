using Features.Interfaces;
using Features.Start.Services;
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
        await botService.UseChat(request.ChatId, cancel);

        var user = await userService.GetByUsername(request.Username);

        if (user.IsFailure())
        {
            await botService.SendTextMessageAsync($"Друже, {user.Message}", cancel);
            return Result.Success();
        }

        if (user.Data is null)
        {
            await botService.SendTextMessageAsync("Друже, звернись до @nazikBro для реєстрації :)", cancel);
            return Result.Success();
        }

        await botService.SendTextMessageAsync(
            "Натисни /підписки щоб переглянути доступні чи придбати новеньку", cancel);
        return Result.Success();
    }
}