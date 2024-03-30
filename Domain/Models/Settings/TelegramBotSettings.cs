namespace Domain.Models.Settings;

public record TelegramBotSettings(
    string Url,
    string UpdateRoute,
    string Name,
    string Token)
{
    public const string Position = "TelegramBot";
}