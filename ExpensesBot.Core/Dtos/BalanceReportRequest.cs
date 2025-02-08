namespace ExpensesBot.Core.Dtos;

public class BalanceReportRequest
{
    public Guid Id { get; private set; }
    public long UserId { get; private set; }
    public DateRange RequestedPeriod { get; private set; }
    public string? Category { get; private set; }

    public BalanceReportRequest(long userId, DateRange period, string? category)
    {
        Id = Guid.NewGuid();
        UserId = userId;
        RequestedPeriod = period;
        Category = category;
    }
}