using ExpensesBot.Core.Enums;

namespace ExpensesBot.
    Core.Entities;

public class BalanceChange
{
    public Guid Id { get; private set; }
    public long UserId { get; private set; }
    
    public BalanceChangeTypes Type { get; private set; }
    public string Category { get; private set; }
    public decimal Amount { get; private set; }
    public DateTime CreatedAt { get; private set; }

    public BalanceChange(long userId, BalanceChangeTypes type, string category,decimal amount)
    {
        Id = Guid.NewGuid();
        UserId = userId;
        Type = type;
        Category = category;
        Amount = amount;
        CreatedAt = DateTime.UtcNow;
    }

    public void Edit(string? category, decimal? amount)
    {
        Category = category ?? Category;
        Amount = amount ?? Amount;
    }
}