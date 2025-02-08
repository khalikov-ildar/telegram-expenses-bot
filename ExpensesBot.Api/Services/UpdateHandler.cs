using ExpensesBot.Core;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;

namespace ExpensesBot.Api.Services;

public class UpdateHandler : IUpdateHandler
{
    private readonly ILogger<UpdateHandler> _logger;
    private readonly ITelegramBotClient _bot;
    private readonly MessageHandler.MessageCommandDispatcher _messageCommandDispatcher;
    private readonly CallbackHandler.CallbackCommandDispatcher _callbackCommandDispatcher;

    public UpdateHandler(ITelegramBotClient bot, ILogger<UpdateHandler> logger, MessageHandler.MessageCommandDispatcher messageCommandDispatcher, CallbackHandler.CallbackCommandDispatcher callbackCommandDispatcher)
    {
        _bot = bot;
        _logger = logger;
        _messageCommandDispatcher = messageCommandDispatcher;
        _callbackCommandDispatcher = callbackCommandDispatcher;
    }
    
    public async Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, HandleErrorSource source,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("HandleError: {Exception}", exception);
        if (exception is RequestException)
            await Task.Delay(TimeSpan.FromSeconds(2), cancellationToken);
    }

    public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        await (update switch
        {
            { Message: { } message }                        => OnMessage(message),
            { EditedMessage: { } message }                  => OnMessage(message),
            { CallbackQuery: {} query }                     => OnQuery(query),
            _                                               => UnknownUpdateHandlerAsync(update)
        });
    }
    
    private async Task OnMessage(Message message)
    {
        _logger.LogInformation("Receive message type: {MessageType}", message.Type);
        if (message.Text is null)
            return;
        
        var result = await _messageCommandDispatcher.ProcessMessage(message, _bot);
        
        result.Switch(successMessage =>
        {
            _logger.LogInformation("Message was sent with id: {Id}", successMessage.Id);
        },  async (errors) =>
        {
            var errorMessage = await _bot.SendMessage(message.Chat, ResultConverter.Problem(errors));
            _logger.LogInformation("Error Message was sent with id: {Id}", errorMessage.Id);
        });
    }

    private async Task OnQuery(CallbackQuery query)
    {
        _logger.LogInformation("Received inline keyboard callback from: {CallbackQueryId}", query.Id);
        await _callbackCommandDispatcher.ProcessCallbackQuery(query);

    }
    
    private Task UnknownUpdateHandlerAsync(Update update)
    {
        _logger.LogInformation("Unknown update type: {UpdateType}", update.Type);
        return Task.CompletedTask;
    }
}