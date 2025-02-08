using ErrorOr;
using ExpensesBot.Core.Commands;
using ExpensesBot.Core.Enums;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace ExpensesBot.Api.Services.MessageHandler.Handlers;

public class CallbackMapper
{
    private readonly Dictionary<string, Func<Chat, string, Task<Message>>> _mapping;
    private readonly ITelegramBotClient _bot;

    public CallbackMapper(ITelegramBotClient bot)
    {
        _bot = bot;
        _mapping = new Dictionary<string, Func<Chat, string, Task<Message>>>()
        {
            {
                nameof(CommandDeleteBalanceHandler), async (chat, msg) =>
                {
                    var replyMarkup =
                        KeyboardMarkupCreator.CreateNegativeBalanceApprovalMarkup( Languages.Ru);
                    return await _bot.SendMessage(chat, msg, replyMarkup: replyMarkup);
                }
            },
            {
                nameof(CommandReportRequestHandler), async (chat, msg) =>
                {
                    var replyMarkup = KeyboardMarkupCreator.CreateExportOptionMarkup(Languages.Ru);
                    return await _bot.SendMessage(chat, msg, replyMarkup: replyMarkup);
                }
            }
        };
    }

    public async Task<Message> MapCallback(string handlerName,Chat chat, string message)
    {
        if (_mapping.TryGetValue(handlerName, out var handler))
        {

            return await handler.Invoke(chat, message);
        }
        
        return await _bot.SendMessage(chat, message);
    }
}