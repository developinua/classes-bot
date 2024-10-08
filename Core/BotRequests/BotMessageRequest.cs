using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Core.BotRequests;

public abstract class BotMessageRequest
{
    protected abstract string Name { get; }
    
    public virtual bool Contains(Message message) =>
        message.Type == MessageType.Text
        && !string.IsNullOrWhiteSpace(message.Text)
        && message.Text.Contains(Name);
}