using ExpensesBot.Core;
using ExpensesBot.Core.Dtos;
using ExpensesBot.Core.Enums;

namespace ExpensesBot.Api.Services.MessageHandler;

public static class MessageParser
{
    public static string ExtractCommand(string message) => message.Split(" ")[0];
    
    public static bool TryParseCategoryWithChangeAmountFromString(string message, out (string Category, decimal Amount) result)
    {
        var stripped = StripCommandFromString(message);
        var category = string.Join(" ",  stripped.SkipLast(1));
        var amountString = stripped.TakeLast(1).ToArray()[0];
        result = (category, 0);

        if (!decimal.TryParse(amountString, out var parsingResult))
        {
            return false;
        }
        var decimalAmount = parsingResult % 1 == 0 ? parsingResult + 0.0m : parsingResult;

        result.Amount = decimalAmount;
        return true;
    }
    
    public static bool TryParseDateRangeWithCategoryFromString(string message, out (DateRange DateRange, string? Category) result)
    {
        var stripped = StripCommandFromString(message);

        var today = DateOnly.FromDateTime(DateTime.Now);
        var monthAgo = today.AddMonths(-1);

        var dr = DateRange.Create(monthAgo, today);
        
        result = (dr!, null);
        
        if (stripped.Length == 0)
        {
            return true;
        }

        var dateRangeString = stripped[0];
        if (dateRangeString.Length != 10 + 1 + 10 || !dateRangeString.Contains('-') || !dateRangeString.Contains('.'))
        {
            return false;
        }

        var parsedDateRange = DateRange.Create(dateRangeString);

        if (parsedDateRange is null)
        {
            return false;
        }

        result.DateRange = parsedDateRange;

        if (stripped.Length != 2) return true;
        
        result.Category = stripped[1];
        return true;

    }

    public static bool TryParseEditPayloadFromString(string message, Languages language, out EditPayload editPayload)
    {
        var stripped = StripCommandFromString(message);
        var stringId = stripped[0];

        editPayload = new EditPayload(Guid.Empty, null, null);

        if (!Guid.TryParse(stringId, out var id))
        {
            return false;
        }

        var parsedCategory = TryParseCategoryForEdit(stripped, language, out var category);
        var parsedAmount = TryParseAmountForEdit(stripped, language, out var amount);

        if (!parsedCategory && !parsedAmount)
        {
            return false;
        }

        editPayload.ChangeId = id;
        editPayload.Category = category;
        editPayload.Amount = amount;
        return true;
    }

    private static bool TryParseCategoryForEdit(string[] stripped, Languages language, out string category)
    {
        category = string.Empty;
        if (language == Languages.En)
        {
            if (!stripped.Contains(Keywords.Category))
            {
                return false;
            }

            category = stripped[Array.IndexOf(stripped, Keywords.Category) + 1];
            return true;
        }
        if (!stripped.Contains(Keywords.Категория))
        {
            return false;
        }

        category = stripped[Array.IndexOf(stripped, Keywords.Категория) + 1];
        return true;
    }
    
    private static bool TryParseAmountForEdit(string[] stripped, Languages language, out decimal amount)
    {
        amount = 0;
        if (language == Languages.En)
        {
            if (!stripped.Contains(Keywords.Amount))
            {
                return false;
            }

            if (!decimal.TryParse(stripped[Array.IndexOf(stripped, Keywords.Amount) + 1], out var parsingResult1))
            {
                return false;
            }

            amount = parsingResult1 % 1 == 0 ? parsingResult1 + 0.0m : parsingResult1;
            return true;
        }
        if (!stripped.Contains(Keywords.Сумма))
        {
            return false;
        }
        
        if (!decimal.TryParse(stripped[Array.IndexOf(stripped, Keywords.Сумма) + 1], out var parsingResult))
        {
            return false;
        }

        amount = parsingResult % 1 == 0 ? parsingResult + 0.0m : parsingResult;
        return true;
    }
    
    public static bool TryParseChangeIdForDeleteFromString(string message, out Guid id)
    {
        id = Guid.Empty;
        var stripped = StripCommandFromString(message);

        if (!Guid.TryParse(stripped[0], out var result)) return false;
        
        id = result;
        return true;

    }
    
    private static string[] StripCommandFromString(string message)
    {
        return message.Split(" ").Skip(1).ToArray();
    }
    
}

