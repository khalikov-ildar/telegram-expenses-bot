using ErrorOr;
using ExpensesBot.Core.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace ExpensesBot.Api.Services.MessageHandler;

public static class KeyboardMarkupCreator
{
    public static IReplyMarkup CreateExportOptionMarkup(Languages language = Languages.En)
    {
        var textVariant = language == Languages.En ? "Text" : "Текст";
        var csv = ReportExportTypes.Csv.ToString();
        var xlsx = ReportExportTypes.Xlsx.ToString();

        return new InlineKeyboardMarkup()
            .AddButton(textVariant, ReportExportTypes.Text.ToString())
            .AddButton(csv, csv)
            .AddButton(xlsx, xlsx);
    }

    public static IReplyMarkup CreateNegativeBalanceApprovalMarkup(Languages language = Languages.En)
    {
        var yes = language == Languages.En ? "Yes" : "Да";
        var no = language == Languages.En ? "No" : "Нет";

        return new InlineKeyboardMarkup()
            .AddButton(yes, NegativeBalanceChangeResponse.Yes.ToString())
            .AddButton(no, NegativeBalanceChangeResponse.No.ToString());
    }
}