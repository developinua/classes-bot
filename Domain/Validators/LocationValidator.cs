﻿using FluentValidation;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Domain.Validators;

public class LocationValidator : AbstractValidator<Message>
{
    public LocationValidator()
    {
        RuleFor(x => x)
            .Must(x =>
                // x?.ReplyToMessage?.From?.Username == "ClassesBot" &&
                x.Type.Equals(MessageType.Location)
                && x.Location != null)
            .WithMessage("Must be valid location");
    }
}