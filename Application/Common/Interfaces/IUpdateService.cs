using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using ResultNet;
using Telegram.Bot.Types;

namespace Application.Common.Interfaces;

public interface IUpdateService
{
    long GetChatId(Update update);
    string? GetUsername(Update update);
    string? GetUsername(CallbackQuery? callbackQuery);
    string? GetUserCultureName(Message? message);
    IRequest<Result>? GetRequestFromUpdate(Update update);
    Task HandleSuccessResponse(long chatId);
    Task HandleFailureResponse(long chatId, CancellationToken cancel, string? responseMessage = null);
    Task<Update?> GetUpdateFromHttpRequest(HttpContext httpContext);
}