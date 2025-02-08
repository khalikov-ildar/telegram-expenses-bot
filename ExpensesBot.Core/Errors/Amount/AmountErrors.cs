using ErrorOr;
using ExpensesBot.Core.Enums;
using static ExpensesBot.Core.Enums.Languages;

namespace ExpensesBot.Core.Errors.Amount;

public static class AmountErrors
{
    public static Error AmountIsNotANumber(Languages language = Languages.En) => AmountIsNotANumberError.Create(language);

    public static Error AmountIsNegativeOrZero(Languages language) => AmountIsNegativeOrZeroError.Create(language);
}

internal static class AmountIsNotANumberError
{
    private const string Code = "AmountIsNotANumberCode";
    private const string EnDescription = "The provided amount value is not a number";
    private const string RuDescription = "Введённое значение на позиции суммы не является числом";

    internal static Error Create(Languages language)
    {
        return Error.Validation(Code, language == Languages.En ? EnDescription : RuDescription);
    }
}

internal static class AmountIsNegativeOrZeroError
{
    private const string Code = "AmountIsNegativeOrZeroCode";
    private const string EnDescription = "The provided amount is 0 or less than 0";
    private const string RuDescription = "Введённое значение суммы меньше либо равно 0";

    internal static Error Create(Languages language)
    {
        return Error.Validation(Code, language == Languages.En ? EnDescription : RuDescription);
    }
}