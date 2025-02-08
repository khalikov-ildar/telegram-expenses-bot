using System.Text;
using ExpensesBot.Core;
using ExpensesBot.Core.Enums;

namespace ExpensesBot.Infrastructure.Services;

public class TextReporter : IReportExporter
{
    public async Task<Stream> Generate(BalanceReport report)
    {
        return GenerateTextStream(report);
    }

    private Stream GenerateTextStream(BalanceReport report)
    {
        var reportMessageBuilder = new StringBuilder();
        
        for (var i = 0; i < report.Changes.Count; i++)
        {
            var change = report.Changes[i];
            var check = change.Type == BalanceChangeTypes.Add ? (sign: '+', amount: change.Amount) : (sign: '-', amount: -change.Amount);
            reportMessageBuilder.Append($"{i + 1}) Id: {change.Id} {check.sign}{change.Amount}, category: {change.Category}, on {DateOnly.FromDateTime(change.CreatedAt)}\n");
        }
        
        var totalMessage = $"Total change is {report.ChangeAmount}";
        reportMessageBuilder.Append(totalMessage);

        var message = reportMessageBuilder.ToString();

        var byteArray = Encoding.UTF8.GetBytes(message);

        return new MemoryStream(byteArray);
    }
}