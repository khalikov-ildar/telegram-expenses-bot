using ExpensesBot.Core.Repositories;
using ExpensesBot.Core.Services;
using ExpensesBot.Infrastructure.Repositories;
using ExpensesBot.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;

namespace ExpensesBot.Infrastructure;

public static class DiExtension
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddSingleton<IUserRepository, UserRepository>();
        services.AddSingleton<IBalanceChangeRepository, BalanceChangeRepository>();
        services.AddSingleton<IBalanceReportRequestRepository, BalanceReportRequestRepository>();
        services.AddSingleton<IBalanceChangeEditRepository, BalanceChangeEditRepository>();
        services.AddSingleton<IBalanceChangeDeleteRequestRepository, BalanceChangeDeleteRequestRepository>();
        services.AddSingleton<IReportDataExporter, ReportDataExporter>();
        services.AddScoped<IContextProvider, TelegramContextProvider>();
        return services;
    }
}