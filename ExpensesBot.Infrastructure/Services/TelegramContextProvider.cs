using ErrorOr;
using ExpensesBot.Core.Services;

namespace ExpensesBot.Infrastructure.Services;

public class TelegramContextProvider : IContextProvider
{
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

    public void RegisterMessageText(string messageText)
    {
        MessageText = messageText;
    }

    public void RegisterUserId(long? userId)
    {
        UserId = userId;
    }
}