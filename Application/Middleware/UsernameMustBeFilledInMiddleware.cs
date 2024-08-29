using System.Threading.Tasks;
using Features.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Application.Middleware;

public class UsernameMustBeFilledInMiddleware(
        IUpdateService updateService,
        IBotService botService,
        ILogger<UsernameMustBeFilledInMiddleware> logger)
    : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var update = await updateService.GetUpdateFromHttpRequest(context);

        if (update is null)
        {
            await next(context);
            return;
        }

        var username = updateService.GetUsername(update);

        if (!string.IsNullOrEmpty(username))
        {
            await next(context);
            return;
        }

        var chatId = updateService.GetChatId(update);

        botService.UseChat(chatId);
        await botService.SendTextMessageAsync("UsernameIsNotFilledIn", context.RequestAborted);
        logger.LogWarning("Username is not filled in for chat {ChatId}", chatId);

        context.Response.StatusCode = 200;
    }
}