using ErrorOr;
using ExpensesBot.Core.Interfaces;

namespace ExpensesBot.Api.Services.Common;


public static class OutputFactory<T>
{
    public static IHandlerOutput<T> Create(ErrorOr<T> result, string handlerName, bool needsCallback = false)
    {
        
        return  new HandlerOutput<T>(result, needsCallback, handlerName);
    }
}
