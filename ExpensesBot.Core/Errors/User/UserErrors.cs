using ErrorOr;
using ExpensesBot.Core.Enums;

namespace ExpensesBot.Core.Errors.User;

public static class UserErrors
{
    public static Error UserIsNotInitialized(Languages language = Languages.En) =>
        UserIsNotInitializedError.Created(language);
    
    public static Error DontHavePermissionForAction(Languages language = Languages.En) =>
        DontHavePermissionForActionError.Created(language);
}

internal static class UserIsNotInitializedError
{
    private const string Code = "UserObjectNotPopulated";
    private const string EnDescription = "Please type \"/start\" command to initialize your account";
    private const string RuDescription = "Пожалуйста введите команду \"/start\" для инициализации вашего аккаунта";

    internal static Error Created(Languages language) =>
        Error.Unauthorized(Code, language == Languages.En ? EnDescription : RuDescription);
}

internal static class DontHavePermissionForActionError
{
    private const string Code = "DontHavePermissionsForActionError";
    private const string EnDescription = "You don't have permission for this action";
    private const string RuDescription = "Вы не имеете права на это действие";

    internal static Error Created(Languages language) =>
        Error.Forbidden(Code, language == Languages.En ? EnDescription : RuDescription);
}