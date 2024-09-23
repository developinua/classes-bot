using System.Threading;
using System.Threading.Tasks;
using Features.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ResultNet;
using Telegram.Bot.Types;

namespace Gateway.Controller;

[Route("api/v1/[controller]")]
public class MessagesController(IUpdateService updateService, IMediator mediator) : ControllerBase
{
    [HttpPost("update")]
    public async Task<IResult> Update([FromBody] Update update, CancellationToken cancel)
    {
        var chatId = updateService.GetChatId(update);
        var request = updateService.GetResultRequest(update);

        if (request is null)
        {
            await updateService.HandleFailureResponse(chatId, cancel);
            return Results.Ok();
        }

        var response = await mediator.Send(request, cancel);

        if (response.IsFailure())
        {
            await updateService.HandleFailureResponse(chatId, cancel, response.Message);
            return Results.Ok();
        }

        await updateService.HandleSuccessResponse(chatId);
        return Results.Ok();
    }
}