using ErrorOr;
using ExpensesBot.Core.Interfaces;

namespace ExpensesBot.Api.Services.Common;

public record HandlerOutput<T>(ErrorOr<T> Result, bool NeedsCallback, string HandlerName) : IHandlerOutput<T>;