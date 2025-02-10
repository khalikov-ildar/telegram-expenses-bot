using ErrorOr;
using ExpensesBot.Core.Enums;

namespace ExpensesBot.Core.Errors.Balance;


public static class BalanceErrors
{
    public static Error BalanceMustBeGreaterThanChangeAmount(decimal balance, decimal changeAmount,
        Languages language = Languages.En) =>
        BalanceMustBeGreaterThanChangeAmountError.Create(balance, changeAmount, language);
    
    public static Error BalanceChangeIdIsNotValid(
        Languages language = Languages.En) =>
        BalanceChangeIdIsNotValidError.Create(language);
    
    public static Error BalanceChangeWasNotFound(Guid id,
        Languages language = Languages.En) =>
        BalanceChangeWasNotFoundError.Create(id,language);
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

internal static class BalanceChangeIdIsNotValidError
{
    private const string Code = "BalanceChangeIdIsNotValid";
        
    internal static Error Create( Languages language)
    {
        const string enMessage = $"Provided Id is of invalid format";
        const string ruMessage = $"Предоставленный идентификатор не действителен";

        return Error.Validation(Code, language == Languages.En ? enMessage : ruMessage);
    }    
}

internal static class BalanceChangeWasNotFoundError
{
    private const string Code = "BalanceChangeIdIsNotValid";
        
    internal static Error Create(Guid id, Languages language)
    {
        var enMessage = $"The balance change with id: \"{id}\" was not found";
        var ruMessage = $"Запись изменения баланса с идентификатором: \"{id}\" не найдена";

        return Error.Validation(Code, language == Languages.En ? enMessage : ruMessage);
    }    
}