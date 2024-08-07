using Core.BotRequests;
using MediatR;
using ResultNet;

namespace Features.Administration;

public class AdminRequest : BotMessageRequest, IRequest<Result>
{
    protected override string Name => "/admin";
    public long ChatId { get; set; }
    public string Username { get; set; } = null!;
}