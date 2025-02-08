using ErrorOr;

namespace ExpensesBot.Core.Services;

public interface IContextProvider
{
    ErrorOr<long> GetUserId();
    string GetMessageText();

    void RegisterMessageText(string messageText);

    void RegisterUserId(long? userId);
}