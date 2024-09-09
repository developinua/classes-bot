using Features.Interfaces;
using MediatR;
using ResultNet;

namespace Features.Administration;

public class AdminHandler(IBotService botService) : IRequestHandler<AdminRequest, Result>
{
    public async Task<Result> Handle(AdminRequest request, CancellationToken cancel)
    {
        await botService.UseChat(request.ChatId, cancel);
        
        if (!CanExecuteCommand(request.Username))
        {
            await botService.SendTextMessageAsync("Access denied. You can't execute this command.", cancel);
        }

        var responseMessage = $"/ініціалізація-даних /змінити-посилання-на-оплату /керувати-підписками";
        
        return Result.Success();
    }

    // todo: extract to permissions
    private static bool CanExecuteCommand(string username)
    {
        var allowedUsers = new[] { "nazikBro", "kovalinas" };
        return allowedUsers.Any(x => x.Equals(username));
    }
}