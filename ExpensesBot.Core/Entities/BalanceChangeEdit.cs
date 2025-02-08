namespace ExpensesBot.Core.Entities;

public class BalanceChangeEdit
{
    public Guid Id { get; private set; }
    public Guid BalanceChangeId { get; private set; }
    public long UserId { get; private set; }

    public BalanceChangeEdit(Guid changeId, long userId)
    {
        Id = Guid.NewGuid();
        BalanceChangeId = changeId;
        UserId = userId;
    }
}