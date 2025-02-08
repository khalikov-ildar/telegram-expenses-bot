using ErrorOr;
using ExpensesBot.Core.Enums;

namespace ExpensesBot.Core.Errors.Balance;


public static class BalanceErrors
{
    public static Error BalanceMustBeGreaterThanChangeAmount(decimal balance, decimal changeAmount,
        Languages language = Languages.En) =>
        BalanceMustBeGreaterThanChangeAmountError.Create(balance, changeAmount, language);
}

internal static class BalanceMustBeGreaterThanChangeAmountError
{
    private const string Code = "BalanceMustBeGreaterThanChangeAmount";

    internal static Error Create(decimal balance, decimal changeAmount, Languages language)
    {
        var enMessage = $"Your balance({balance}) must be greater than subtraction amount({changeAmount})";
        var ruMessage = $"Ваш баланс({balance}) должен быть больше суммы вычета({changeAmount})";

        return Error.Validation(Code, language == Languages.En ? enMessage : ruMessage);
    }
}