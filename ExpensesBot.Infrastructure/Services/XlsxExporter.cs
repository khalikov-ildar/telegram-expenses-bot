using System.Globalization;
using ExpensesBot.Core;
using OfficeOpenXml;

namespace ExpensesBot.Infrastructure.Services;

public class XlsxExporter : IReportExporter
{
    public async Task<Stream> Generate(BalanceReport report)
    {
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

        using var package = new ExcelPackage();
        var worksheet = package.Workbook.Worksheets.Add("Balance Changes Report");

        worksheet.Cells[1, 1].Value = "Id";
        worksheet.Cells[1, 2].Value = "Category";
        worksheet.Cells[1, 3].Value = "Amount";
        worksheet.Cells[1, 4].Value = "Type";
        worksheet.Cells[1, 5].Value = "Date";

        for (var i = 0; i < report.Changes.Count; i++)
        {
            var change = report.Changes[i];
            worksheet.Cells[i + 2, 1].Value = change.Id;
            worksheet.Cells[i + 2, 2].Value = change.Category;
            worksheet.Cells[i + 2, 3].Value = change.Amount;
            worksheet.Cells[i + 2, 4].Value = change.Type.ToString();
            worksheet.Cells[i + 2, 5].Value = change.CreatedAt.ToString(CultureInfo.InvariantCulture);

        }

        worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

        var stream = new MemoryStream();
        await package.SaveAsAsync(stream);

        stream.Position = 0;

        return stream;
    }
}