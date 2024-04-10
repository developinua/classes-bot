using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace Infrastructure.Bot;

public class BotService(ITelegramBotClient botClient) : IBotService
{
    private long ChatId { get; set; }

    public void UseChat(long chatId) => ChatId = chatId;

    public async Task<Message> SendTextMessageAsync(
        string text,
        CancellationToken cancel,
        ParseMode parseMode)
    {
        return await botClient.SendTextMessageAsync(
            ChatId,
            text.Replace("{newline}", Environment.NewLine),
            parseMode: parseMode,
            cancellationToken: cancel);
    }

    public async Task<Message> SendTextMessageWithReplyAsync(
        string text,
        IReplyMarkup replyMarkup,
        CancellationToken cancel,
        ParseMode parseMode)
    {
        return await botClient.SendTextMessageAsync(
            ChatId,
            text.Replace("{newline}", Environment.NewLine),
            parseMode: parseMode,
            replyMarkup: replyMarkup,
            cancellationToken: cancel);
    }

    public async Task SendChatActionAsync(
        CancellationToken cancel,
        ChatAction typing)
    {
        await botClient.SendChatActionAsync(ChatId, chatAction: typing, cancellationToken: cancel);
    }
}