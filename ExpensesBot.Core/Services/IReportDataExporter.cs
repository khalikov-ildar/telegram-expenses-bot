using ExpensesBot.Core.Enums;

namespace ExpensesBot.Core.Services;

public interface IReportDataExporter
{
    public Task<Stream> Generate(BalanceReport report, ReportExportTypes type);
}