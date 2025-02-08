using ExpensesBot.Core;
using ExpensesBot.Core.Enums;
using ExpensesBot.Core.Services;

namespace ExpensesBot.Infrastructure.Services;

public class ReportDataExporter : IReportDataExporter
{
    private readonly Dictionary<ReportExportTypes, IReportExporter> _exporters;

    public ReportDataExporter()
    {
        _exporters = new Dictionary<ReportExportTypes, IReportExporter>
        {
            { ReportExportTypes.Text, new TextReporter() },
            { ReportExportTypes.Csv, new CsvExporter() },
            { ReportExportTypes.Xlsx, new XlsxExporter() }
        };
    }

    public async Task<Stream> Generate(BalanceReport report, ReportExportTypes type)
    {
        if (!_exporters.TryGetValue(type, out var exporter))
        {
            throw new NotSupportedException($"File export type '{type}' is not supported");
        }

        return await exporter.Generate(report);
    }
}