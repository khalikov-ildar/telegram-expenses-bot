using ExpensesBot.Core.Entities;
using ExpensesBot.Core.Repositories;

namespace ExpensesBot.Infrastructure.Repositories;

public class BalanceChangeEditRepository : IBalanceChangeEditRepository
{
    private List<BalanceChangeEdit> _edits = [];
    
    public async Task Add(BalanceChangeEdit changeEdit)
    {
        _edits.Add(changeEdit);
    }

    public async Task<BalanceChangeEdit?> GetLastByUserId(long userId)
    {
        return _edits.FirstOrDefault(e => e.UserId == userId);
    }

    public async Task DeleteById(Guid id)
    {
        _edits = _edits.FindAll(e => e.Id != id);
    }
}