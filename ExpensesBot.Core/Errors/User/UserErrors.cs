using ErrorOr;
using ExpensesBot.Core.Enums;

namespace ExpensesBot.Core.Errors.User;

public static class UserErrors
{
    public static Error UserObjectWasNotPopulated(Languages language = Languages.En) =>
        UserObjectWasNotPopulatedError.Created(language);
}

internal static class UserObjectWasNotPopulatedError
{
    private const string Code = "UserObjectNotPopulated";
    private const string EnDescription = "Please type \"/start\" command to initialize your account";
    private const string RuDescription = "Пожалуйста введите команду \"/start\" для инициализации вашего аккаунта";

    internal static Error Created(Languages language) =>
        Error.Unexpected(Code, language == Languages.En ? EnDescription : RuDescription);
}