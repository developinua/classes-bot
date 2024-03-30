using Domain.Requests.Bot;
using MediatR;
using ResultNet;

namespace Domain.Requests;

public class AdminRequest : BotMessageRequest, IRequest<Result>
{
    protected override string Name => "/admin";
    public long ChatId { get; set; }
    public string Username { get; set; } = null!;
}