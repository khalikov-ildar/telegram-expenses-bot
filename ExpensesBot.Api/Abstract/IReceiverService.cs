namespace ExpensesBot.Api.Abstract;

public interface IReceiverService
{
    Task ReceiveAsync(CancellationToken ct);
}