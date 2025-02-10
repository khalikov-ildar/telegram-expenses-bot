using ErrorOr;
using ExpensesBot.Core.Enums;

namespace ExpensesBot.Core.Services;

public interface IContextProvider
{
    ErrorOr<long> GetUserId();
    string GetMessageText();

    Languages GetUserLanguage(long userId);
    
    void RegisterMessageText(string messageText);

    void RegisterUserId(long? userId);

    void RegisterUserLanguage(long userId,Languages language);
}