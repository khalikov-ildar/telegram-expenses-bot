using ErrorOr;
using ExpensesBot.Core.Enums;

namespace ExpensesBot.Core.Errors.DateRange;

public static class DateRangeErrors
{
    public static Error DateRangeIsWrongFormatted(Languages language = Languages.En) =>
        DateRangeIsWrongFormattedError.Create(language);

    public static Error DateRangeStartLaterThanEnd(Languages language = Languages.En) =>
        DateRangeStartLaterThanEndError.Create(language);
}

internal static class DateRangeIsWrongFormattedError
{
    private const string Code = "DateRangeWrongFormat";
    private const string EnDescription =
        "Wrong format of date range is provided. Use \"/report-example\" for example of correct format of message";
    private const string RuDescription =
        "Неверный формат временного периода. Используйте \"/пример-отчета\" для примера правильного формата временного периода";

    internal static Error Create(Languages language) =>
        Error.Validation(Code, language == Languages.En ? EnDescription : RuDescription);
}


internal static class DateRangeStartLaterThanEndError
{
    private const string Code = "DateRangeStartLaterThanEnd";
    private const string EnDescription = "The starting date of requested report should not be later than the end";
    private const string RuDescription = "Начальная дата отчета должна быть не позже даты конца";

    internal static Error Create(Languages language) =>
        Error.Validation(Code, language == Languages.En ? EnDescription : RuDescription);
}