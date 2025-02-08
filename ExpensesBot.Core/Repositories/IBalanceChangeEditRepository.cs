using ExpensesBot.Core.Entities;

namespace ExpensesBot.Core.Repositories;

public interface IBalanceChangeEditRepository
{
    Task Add(BalanceChangeEdit changeEdit);
    Task<BalanceChangeEdit?> GetLastByUserId(long userId);
    Task DeleteById(Guid id);
}