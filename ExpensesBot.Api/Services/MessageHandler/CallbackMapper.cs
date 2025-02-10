using ExpensesBot.Core.Commands;
using ExpensesBot.Core.Enums;
using ExpensesBot.Core.Services;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace ExpensesBot.Api.Services.MessageHandler;

public class CallbackMapper
{
    private readonly Dictionary<string, Func<Chat, string, Languages, Task<Message>>> _mapping;
    private readonly ITelegramBotClient _bot;

    public CallbackMapper(ITelegramBotClient bot)
    {
        _bot = bot;
        _mapping = new Dictionary<string, Func<Chat, string, Languages, Task<Message>>>()
        {
            {
                nameof(CommandDeleteBalanceHandler), async (chat, msg, language) =>
                {
                    var replyMarkup =
                        KeyboardMarkupCreator.CreateNegativeBalanceApprovalMarkup(language);
                    return await _bot.SendMessage(chat, msg, replyMarkup: replyMarkup);
                }
            },
            {
                nameof(CommandReportRequestHandler), async (chat, msg, language) =>
                {
                    Console.WriteLine(language.ToString());
                    var replyMarkup = KeyboardMarkupCreator.CreateExportOptionMarkup(language);
                    return await _bot.SendMessage(chat, msg, replyMarkup: replyMarkup);
                }
            }
        };
    }

    public async Task<Message> MapCallback(string handlerName,Chat chat, string message, Languages language)
    {
        if (_mapping.TryGetValue(handlerName, out var handler))
        {

            return await handler.Invoke(chat, message, language);
        }
        
        return await _bot.SendMessage(chat, message);
    }
}