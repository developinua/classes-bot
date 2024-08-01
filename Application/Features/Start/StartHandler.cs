using System.Threading;
using System.Threading.Tasks;
using Application.Features.Language;
using Application.Interfaces;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Localization;
using ResultNet;

namespace Application.Features.Start;

public class StartHandler(
        IBotService botService,
        IUserService userService,
        IUpdateService updateService,
        ICultureService cultureService,
        IStringLocalizer<StartHandler> localizer,
        IMediator mediator,
        IMapper mapper)
    : IRequestHandler<StartRequest, Result>
{
    public async Task<Result> Handle(StartRequest request, CancellationToken cancel)
    {
        botService.UseChat(request.ChatId);
        await botService.SendChatActionAsync(cancel);

        if (await userService.UserAlreadyRegistered(request.Username))
        {
            await botService.SendTextMessageAsync(localizer.GetString("BotAlreadyStarted"), cancel);
            return Result.Success();
        }

        var botUserCultureName = updateService.GetUserCultureName(request.Message);
        var culture = await cultureService.GetByName(botUserCultureName);
        var result = await userService.SaveUser(request.Username, request.Message, culture, cancel);
        
        if (result.IsFailure()) return result;

        await mediator.Send(mapper.Map<LanguageRequest>(request), cancel);
        return Result.Success();
    }
}