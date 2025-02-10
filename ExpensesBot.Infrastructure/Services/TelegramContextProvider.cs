using ErrorOr;
using ExpensesBot.Core.Enums;
using ExpensesBot.Core.Services;

namespace ExpensesBot.Infrastructure.Services;

public class TelegramContextProvider : IContextProvider
{
    private readonly ILanguageProvider _languageProvider;

    public TelegramContextProvider(ILanguageProvider languageProvider)
    {
        _languageProvider = languageProvider;
    }

    private long? UserId { get; set; }
    private string MessageText { get; set; } = string.Empty;
    
    public ErrorOr<long> GetUserId()
    {
        return UserId is null
            ? Error.Unexpected("", "Something went wrong. Try again later")
            : UserId.Value;
    }

    public string GetMessageText()
    {
        return MessageText;
    }

    public Languages GetUserLanguage(long userId)
    {
        return _languageProvider.GetUserLanguage(userId);
    }

    public void RegisterMessageText(string messageText)
    {
        MessageText = messageText;
    }

    public void RegisterUserId(long? userId)
    {
        UserId = userId;
    }

    public void RegisterUserLanguage(long userId, Languages language)
    {
        _languageProvider.RegisterLanguage(userId, language);
    }
}