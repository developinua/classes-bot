using Features.Interfaces;
using MediatR;
using ResultNet;

namespace Features.Administration;

public class AdminHandler(IBotService botService) : IRequestHandler<AdminRequest, Result>
{
    public async Task<Result> Handle(AdminRequest request, CancellationToken cancel)
    {
        botService.UseChat(request.ChatId);
        
        if (!CanExecuteCommand(request.Username))
        {
            await botService.SendTextMessageAsync("Access denied. You can't execute this command.", cancel);
        }

        var responseMessage = $"/seed /payment-link /manage-subscriptions";
        
        return Result.Success();
    }

    // todo: extract to permissions
    private static bool CanExecuteCommand(string username)
    {
        var allowedUsers = new[] { "nazikBro", "taras_zouk", "kovalinas" };
        return allowedUsers.Any(x => x.Equals(username));
    }
}