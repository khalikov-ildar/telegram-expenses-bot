using ErrorOr;

namespace ExpensesBot.Core.Interfaces;

public interface IHandlerOutput<T>
{
    ErrorOr<T> Result { get; }
    string HandlerName { get; }
    bool NeedsCallback { get; }
}