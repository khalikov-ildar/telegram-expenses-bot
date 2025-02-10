using ExpensesBot.Core.Enums;
using ExpensesBot.Core.Services;

namespace ExpensesBot.Infrastructure.Services;

public class LanguageProvider : ILanguageProvider
{
    private readonly Dictionary<long, Languages> _userLanguagesMap = new();
    
    public void RegisterLanguage(long userId, Languages language)
    {
        _userLanguagesMap[userId] = language;
    }

    public Languages GetUserLanguage(long userId)
    {
        _userLanguagesMap.TryGetValue(userId, out var language);
        return language;
    }
}