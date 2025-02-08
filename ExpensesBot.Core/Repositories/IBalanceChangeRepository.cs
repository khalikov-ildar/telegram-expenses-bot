using ExpensesBot.Core.Entities;

namespace ExpensesBot.Core.Repositories;

public interface IBalanceChangeRepository
{
    Task AddBalanceChange(BalanceChange balanceChange);
    Task <BalanceChange?> GetBalanceChangeById(Guid id);
    Task<List<BalanceChange>> ListAllInDateRange(DateRange range, string? category);

    Task DeleteBalanceChangeById(Guid id);
}