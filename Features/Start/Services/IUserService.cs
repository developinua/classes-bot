using ResultNet;
using User = Core.Aggregates.User.User;

namespace Features.Start.Services;

public interface IUserService
{
    Task<Result<User?>> GetByUsername(string? username);
}