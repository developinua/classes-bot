using MediatR;
using Microsoft.AspNetCore.Http;
using ResultNet;
using Telegram.Bot.Types;

namespace Features.Interfaces;

public interface IUpdateService
{
    long GetChatId(Update update);
    string? GetUsername(Update update);
    string? GetUsername(CallbackQuery? callbackQuery);
    string? GetUserCultureName(Message? message);
    IRequest<Result>? GetResultRequest(Update update);
    Task HandleSuccessResponse(long chatId);
    Task HandleFailureResponse(long chatId, CancellationToken cancel, string? responseMessage = null);
    Task<Update?> GetUpdateFromHttpRequest(HttpContext httpContext);
}