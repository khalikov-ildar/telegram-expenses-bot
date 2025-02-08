namespace ExpensesBot.Core.Entities;

public class BalanceChangeDeleteRequest
{
    public Guid Id { get; private set; }
    public long UserId { get; private set; }
    public Guid ChangeId { get; private set; }

    public BalanceChangeDeleteRequest(long userId, Guid changeId)
    {
        Id = Guid.NewGuid();
        UserId = userId;
        ChangeId = changeId;
    }
}