using System.ComponentModel;
using System.Globalization;

namespace ExpensesBot.Core;

public record DateRange
{
    public DateOnly Start { get; init; }
    public DateOnly End { get; init; }
    
    private DateRange(DateOnly start, DateOnly end)
    {
        Start = start;
        End = end;
    }

    public static DateRange? Create(DateOnly start, DateOnly end)
    {
        return start.CompareTo(end) > 0 ? null : new DateRange(start, end);
    }

    public static DateRange? Create(string dateRangeString)
    {
        var splitted = dateRangeString.Split("-");
        var isStartParsed = DateOnly.TryParseExact(splitted[0], "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var start);
        var isEndParsed = DateOnly.TryParseExact(splitted[1], "dd.MM.yyyy", CultureInfo.InvariantCulture,
            DateTimeStyles.None, out var end);

        if (!isStartParsed || !isEndParsed)
        {
            return null;
        }
        
        return DateRange.Create(start, end);
    }
}