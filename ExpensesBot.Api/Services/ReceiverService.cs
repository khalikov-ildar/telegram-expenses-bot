using ExpensesBot.Api.Abstract;
using Microsoft.Extensions.Logging;
using Telegram.Bot;

namespace ExpensesBot.Api.Services;

public class ReceiverService(
    ITelegramBotClient botClient, UpdateHandler updateHandler, ILogger<ReceiverServiceBase<UpdateHandler>> logger)
    : ReceiverServiceBase<UpdateHandler>(botClient, updateHandler, logger);