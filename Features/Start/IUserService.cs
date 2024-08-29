using ResultNet;
using Telegram.Bot.Types;
using User = Core.Entities.Aggregates.User.User;

namespace Features.Start;

public interface IUserService
{
    Task<Result> SaveUser(string username, Message message, CancellationToken cancel);
    Task<Result<User?>> GetByUsername(string? username);
    Task<bool> UserAlreadyRegistered(string username);
}