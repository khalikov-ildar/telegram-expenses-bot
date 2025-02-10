using ExpensesBot.Core.Enums;

namespace ExpensesBot.Core.Services;

public interface ILanguageProvider
{
    void RegisterLanguage(long userId, Languages language);
    Languages GetUserLanguage(long userId);
}