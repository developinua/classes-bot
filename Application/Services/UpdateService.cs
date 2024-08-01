using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Application.Interfaces;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ResultNet;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Application.Services;

public class UpdateService(
        IMapper mapper,
        IBotService botService,
        ILogger<UpdateService> logger)
    : IUpdateService
{
    public long GetChatId(Update update) =>
        update.Message?.From?.Id ?? update.CallbackQuery!.From.Id;

    public string? GetUsername(Update update) =>
        update.Message?.From?.Username ?? GetUsername(update.CallbackQuery!);

    public string? GetUsername(CallbackQuery? callbackQuery) =>
        callbackQuery?.From.Username ?? callbackQuery?.From.Id.ToString();

    public string? GetUserCultureName(Message? message) => message?.From?.LanguageCode;

    public IRequest<Result>? GetResultRequest(Update update)
    {
        var request = update switch
        {
            { Type: UpdateType.Message } => mapper.Map<IRequest<Result>>(update.Message!),
            { Type: UpdateType.CallbackQuery } => mapper.Map<IRequest<Result>>(update.CallbackQuery),
            _ => null
        };

        if (request is not null) return request;

        logger.LogError(
            "Update handler not found\n" +
            "Update type: {updateType}\n" +
            "Update message type: {messageType}.",
            update.Type,
            update.Message?.Type.ToString() ?? "No message type was specified.");

        return default;
    }

    public async Task<Update?> GetUpdateFromHttpRequest(HttpContext httpContext)
    {
        // // Needed to re-read the stream
        httpContext.Request.EnableBuffering();

        using var reader = new StreamReader(
            httpContext.Request.Body,
            encoding: Encoding.UTF8,
            detectEncodingFromByteOrderMarks: false,
            leaveOpen: true);
        var body = await reader.ReadToEndAsync();

        // Reset the stream position for the next middleware
        httpContext.Request.Body.Position = 0;

        return JsonConvert.DeserializeObject<Update>(body);
    }

    public Task HandleSuccessResponse(long chatId)
    {
        logger.LogInformation(
            "Successful response from chat {ChatId}. Date: {DateTime}.",
            chatId.ToString(),
            DateTime.UtcNow);
        return Task.CompletedTask;
    }

    public async Task HandleFailureResponse(
        long chatId,
        CancellationToken cancel,
        string? responseMessage = null)
    {
        logger.LogError(
            "Chat id: {ChatId}\nMessage:\n{ErrorMessage}.",
            chatId.ToString(),
            responseMessage ?? "No message was specified.");

        botService.UseChat(chatId);
        await botService.SendTextMessageAsync("Can't process the message\\.", cancel);
    }
}