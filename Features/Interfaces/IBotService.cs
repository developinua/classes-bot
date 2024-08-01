using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace Features.Interfaces;

public interface IBotService
{
    void UseChat(long chatId);
    Task<Message> SendTextMessageAsync(
        string text,
        CancellationToken cancel,
        ParseMode parseMode = ParseMode.MarkdownV2);
    Task<Message> SendTextMessageWithReplyAsync(
        string text,
        IReplyMarkup replyMarkup,
        CancellationToken cancel,
        ParseMode parseMode = ParseMode.MarkdownV2);
    Task SendChatActionAsync(CancellationToken cancel, ChatAction typing = ChatAction.Typing);
}