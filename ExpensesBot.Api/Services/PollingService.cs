using ExpensesBot.Api.Abstract;
using Microsoft.Extensions.Logging;

namespace ExpensesBot.Api.Services;

public class PollingService(IServiceProvider serviceProvider, ILogger<PollingService> logger)
    : PollingServiceBase<ReceiverService>(serviceProvider, logger);