using ErrorOr;
using ExpensesBot.Core.Enums;

namespace ExpensesBot.Core.Errors.BalanceReport;

public static class BalanceReportErrors
{
    private const string Code = "BalanceReportWasNotFound";

    public static Error BalanceReportWasNotFoundError(Languages language = Languages.En) =>
        Error.NotFound(Code, language == Languages.En ? "Balance report was not found" : "Отчет не был найден");
}