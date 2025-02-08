using System.Text;
using ExpensesBot.Core;
using ExpensesBot.Core.Services;

namespace ExpensesBot.Infrastructure.Services;

public class CsvExporter : IReportExporter
{
    public async Task<Stream> Generate(BalanceReport report)
    {
        return await GenerateCsv(report);
    }

    private static async Task<Stream> GenerateCsv(BalanceReport report)
    {
        var memoryStream = new MemoryStream();
        var writer = new StreamWriter(memoryStream, Encoding.UTF8, leaveOpen: true);
        await writer.WriteLineAsync("Id,Category,Amount,Type,Date");
        foreach (var change in report.Changes) 
        {
            await writer.WriteLineAsync($"{change.Id},{EscapeCsvValue(change.Category)},{change.Amount},{change.Type.ToString()},{change.CreatedAt}");
        }
            
        await writer.FlushAsync();

        memoryStream.Position = 0;

        return memoryStream;
    }

    private static string EscapeCsvValue(string value)
    {
        if (!value.Contains(',') && !value.Contains('"')) return value;
        
        value = value.Replace("\"", "\"\"");
        value = $"\"{value}\"";
        return value;
    }
}