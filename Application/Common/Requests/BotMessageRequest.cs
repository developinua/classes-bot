using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Application.Common.Requests;

public abstract class BotMessageRequest
{
    protected abstract string Name { get; }
    
    public virtual bool Contains(Message message) =>
        message.Type == MessageType.Text && message.Text!.Contains(Name);
}