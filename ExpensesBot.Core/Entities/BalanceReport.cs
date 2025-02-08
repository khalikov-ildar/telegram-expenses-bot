using System.Text;
using ExpensesBot.Core.Entities;
using ExpensesBot.Core.Enums;

namespace ExpensesBot.Core;

public class BalanceReport
{
    public long UserId { get; private set; }
    public DateRange RequestedPeriod { get; private set; }
    public string? Category { get; private set; }
    
    public List<BalanceChange> Changes { get; private set; }
    public decimal ChangeAmount { get; private set; }

    private BalanceReport(long userId, DateRange period, string? category, decimal changeAmount, List<BalanceChange> changes)
    {
        UserId = userId;
        RequestedPeriod = period;
        Category = category;
        ChangeAmount = changeAmount;
        Changes = changes;
    }

    public static BalanceReport Create(long userId, DateRange dateRange, string? category, List<BalanceChange> changes)
    {
        if (changes.Count == 0)
        {
            return new BalanceReport(userId,
                dateRange,
                category,
                0,
                changes);
        }
        
        var totalAmount = changes.Sum(change => change.Type == BalanceChangeTypes.Add ? change.Amount : -change.Amount);

        return new BalanceReport(userId, dateRange, category, totalAmount, changes);
    }
}