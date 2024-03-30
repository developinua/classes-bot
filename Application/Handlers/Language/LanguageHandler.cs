﻿using System.Threading;
using System.Threading.Tasks;
using Application.Services;
using Domain.Requests;
using MediatR;
using Microsoft.Extensions.Localization;
using ResultNet;

namespace Application.Handlers.Language;

public class LanguageHandler(
        IBotService botService,
        IReplyMarkupService replyMarkupService,
        IStringLocalizer<LanguageHandler> localizer)
    : IRequestHandler<LanguageRequest, Result>
{
    public async Task<Result> Handle(LanguageRequest request, CancellationToken cancellationToken)
    {
        botService.UseChat(request.ChatId);
        await botService.SendChatActionAsync(cancellationToken);

        await botService.SendTextMessageWithReplyAsync(
            localizer["CommunicationLanguage"],
            replyMarkupService.GetStartMarkup(),
            cancellationToken);
        
        return Result.Success();
    }
}