using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Core.Aggregates.User;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Localization;
using ResultNet;
using Telegram.Bot.Types;

namespace Application.Features.Language;

public class LanguageCallbackHandler(
        IBotService botService,
        IUserProfileService userProfileService,
        ICultureService cultureService,
        ICallbackExtractorService callbackExtractorService,
        IStringLocalizer<LanguageHandler> localizer,
        IValidator<CallbackQuery> validator)
    : IRequestHandler<LanguageCallbackRequest, Result>
{
    public async Task<Result> Handle(LanguageCallbackRequest request, CancellationToken cancel)
    {
        if ((await validator.ValidateAsync(request.CallbackQuery, cancel)).IsValid)
            return Result.Failure().WithMessage("Invalid callback query.");

        botService.UseChat(request.ChatId);
        await botService.SendChatActionAsync(cancel);

        var cultureName = callbackExtractorService.GetCultureNameFrom(
            request.CallbackQuery.Data,
            request.CallbackPattern);
        var culture = await cultureService.GetByName(cultureName);
        // todo: use mapster
        var userProfile = new UserProfile
        {
            ChatId = request.CallbackQuery.From.Id,
            FirstName = request.CallbackQuery.From.FirstName,
            LastName = request.CallbackQuery.From.LastName,
            IsPremium = request.CallbackQuery.From.IsPremium.GetValueOrDefault(),
            IsBot = request.CallbackQuery.From.IsBot,
            Culture = culture
        };

        await userProfileService.UpdateUserProfile(userProfile, cancel);
        await botService.SendTextMessageAsync(localizer.GetString("ManageClasses"), cancel);
        return Result.Success();
    }
}