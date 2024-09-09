using Features.Interfaces;
using Microsoft.EntityFrameworkCore;
using ResultNet;
using User = Core.Aggregates.User.User;

namespace Features.Start.Services;

public class UserService(
    IClassesDbContext context)
    : IUserService
{
    public async Task<Result<User?>> GetByUsername(string? username)
    {
        if (string.IsNullOrEmpty(username))
            return Result.Failure<User?>().WithMessage("нікнейм має бути заповнений");

        var response = await context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.NickName == username);

        return response;
    }
}