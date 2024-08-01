using Features.Interfaces;
using MediatR;
using Microsoft.Extensions.Localization;
using ResultNet;

namespace Features.Language;

public class LanguageHandler(
        IBotService botService,
        IReplyMarkupService replyMarkupService,
        IStringLocalizer<LanguageHandler> localizer)
    : IRequestHandler<LanguageRequest, Result>
{
    public async Task<Result> Handle(LanguageRequest request, CancellationToken cancel)
    {
        botService.UseChat(request.ChatId);
        await botService.SendChatActionAsync(cancel);

        await botService.SendTextMessageWithReplyAsync(
            localizer["CommunicationLanguage"],
            replyMarkupService.GetStartMarkup(),
            cancel);
        
        return Result.Success();
    }
}