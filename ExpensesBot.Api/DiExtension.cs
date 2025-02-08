using ExpensesBot.Api.Services.MessageHandler.Handlers;
using Microsoft.Extensions.DependencyInjection;

namespace ExpensesBot.Api;

public static class DiExtension
{
    public static IServiceCollection AddApi(this IServiceCollection services)
    {

        services.AddScoped<CallbackMapper>();
        
        return services;
    }
}
