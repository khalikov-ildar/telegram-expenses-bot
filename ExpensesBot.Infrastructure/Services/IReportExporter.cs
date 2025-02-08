using ExpensesBot.Core;

namespace ExpensesBot.Infrastructure.Services;

public interface IReportExporter
{
    Task<Stream> Generate(BalanceReport report);
}