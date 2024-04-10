using System.Threading;
using System.Threading.Tasks;
using Core.Aggregates.User;
using Core.Entities;
using ResultNet;
using Telegram.Bot.Types;

namespace Application.Features.Start;

public interface IUserService
{
    Task<Result> SaveUser(string username, Message message, Culture culture, CancellationToken cancel);
    Task<Result<BotUser?>> GetByUsername(string? username);
    Task<bool> UserAlreadyRegistered(string username);
}