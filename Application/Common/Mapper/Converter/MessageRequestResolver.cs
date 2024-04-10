using System.Collections.Generic;
using System.Linq;
using Application.Common.Requests;
using AutoMapper;
using MediatR;
using ResultNet;
using Telegram.Bot.Types;

namespace Application.Common.Mapper.Converter;

public class MessageRequestValueConverter(IEnumerable<BotMessageRequest> messageRequests)
    : ITypeConverter<Message, IRequest<Result>?>
{
    public IRequest<Result>? Convert(Message source, IRequest<Result>? destination, ResolutionContext context)
    {
        var messageRequest = messageRequests.SingleOrDefault(x => x.Contains(source));

        if (messageRequest is null) return default;
        
        return context.Mapper.Map(source, messageRequest, typeof(Message), messageRequest.GetType())
            as IRequest<Result>;
    }
}