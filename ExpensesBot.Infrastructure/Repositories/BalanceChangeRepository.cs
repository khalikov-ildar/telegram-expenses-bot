using ExpensesBot.Core;
using ExpensesBot.Core.Entities;
using ExpensesBot.Core.Repositories;

namespace ExpensesBot.Infrastructure.Repositories;

public class BalanceChangeRepository : IBalanceChangeRepository
{
    private readonly List<BalanceChange> _changes = [];

    public async Task AddBalanceChange(BalanceChange balanceChange)
    {
        _changes.Add(balanceChange);
    }

    public Task<BalanceChange?> GetBalanceChangeById(Guid id)
    {
        return Task.FromResult(_changes.FirstOrDefault(c => c.Id == id));
    }

    public Task<List<BalanceChange>> ListAllInDateRange(DateRange range, string? category)
    {
        var startDate = range.Start.ToDateTime(TimeOnly.MinValue); 
        var endDate = range.End.ToDateTime(TimeOnly.MaxValue); 

        return category is null
            ? Task.FromResult(_changes.FindAll(c =>
                c.CreatedAt.Date >= startDate.Date 
                && c.CreatedAt.Date <= endDate.Date))
            : Task.FromResult(_changes.FindAll(c => 
                c.Category == category 
                && c.CreatedAt.Date >= startDate.Date 
                && c.CreatedAt.Date <= endDate.Date));
    }

    public async Task DeleteBalanceChangeById(Guid id)
    {
        var index = _changes.FindIndex(r => r.Id == id);
        _changes.RemoveAt(index);
    }
}