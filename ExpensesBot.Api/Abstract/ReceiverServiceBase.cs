using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Polling;

namespace ExpensesBot.Api.Abstract;

public abstract class ReceiverServiceBase<TUpdateHandler>(
    ITelegramBotClient botClient,
    TUpdateHandler updateHandler,
    ILogger<ReceiverServiceBase<TUpdateHandler>> logger)
    : IReceiverService where TUpdateHandler : IUpdateHandler
{
    public async Task ReceiveAsync(CancellationToken ct)
    {
        var receiverOptions = new ReceiverOptions() { DropPendingUpdates = true, AllowedUpdates = [] };

        var me = await botClient.GetMe(ct);
        logger.LogInformation("Start receiving updates for {BotName}", me.Username ?? "My Awesome Bot");

        // Start receiving updates
        await botClient.ReceiveAsync(updateHandler, receiverOptions, ct);
    }
}