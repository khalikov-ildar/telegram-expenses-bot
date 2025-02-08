using ErrorOr;
using ExpensesBot.Core.Enums;
using ExpensesBot.Core.Errors.User;
using Telegram.Bot.Types;

namespace ExpensesBot.Api.Services;

public static class Validator
{
    public static bool TryGetUserId(Message message, out long userId)
    {
        userId = -1;
        if (message.From is null)
        {
            return false;
        }

        userId = message.From.Id;
        return true;
    }

}