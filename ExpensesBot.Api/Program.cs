using ExpensesBot.Api;
using ExpensesBot.Api.Services;
using ExpensesBot.Api.Services.CallbackHandler;
using ExpensesBot.Api.Services.MessageHandler;
using ExpensesBot.Core;
using ExpensesBot.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Telegram.Bot;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {   
        services.Configure<BotConfiguration>(context.Configuration.GetSection("BotConfiguration"));

        services.AddHttpClient("telegram_bot_client").RemoveAllLoggers()
            .AddTypedClient<ITelegramBotClient>((httpClient, provider) =>
            {
                var botConfiguration = provider.GetService<IOptions<BotConfiguration>>();
                ArgumentNullException.ThrowIfNull(botConfiguration);
                TelegramBotClientOptions options = new(botConfiguration.Value.Token);
                return new TelegramBotClient(options, httpClient);
            });


        services.AddInfrastructure()
            .AddCore()
            .AddApi();
        services.AddScoped<UpdateHandler>();
        services.AddScoped<ReceiverService>();
        services.AddScoped<CallbackCommandDispatcher>();
        services.AddScoped<MessageCommandDispatcher>();
        services.AddHostedService<PollingService>();
    })
    .Build();

await host.RunAsync();