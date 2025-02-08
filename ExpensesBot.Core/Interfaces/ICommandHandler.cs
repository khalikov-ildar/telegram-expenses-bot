namespace ExpensesBot.Core.Interfaces;

public interface ICommandHandler<in TRequest, TResponse>
{
    Task<TResponse> Handle(TRequest request);
}