using Core.BotRequests;
using MediatR;
using ResultNet;

namespace Application.Features.Administration;

public class SeedRequest : BotMessageRequest, IRequest<Result>
{
    protected override string Name => "/seed";
    public long ChatId { get; set; }
    public string Username { get; set; } = null!;
}