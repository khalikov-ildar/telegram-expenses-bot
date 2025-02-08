using ExpensesBot.Core.Entities;

namespace ExpensesBot.Core.Repositories;

public interface IBalanceChangeDeleteRequestRepository
{
    Task SaveChangeDeleteRequest(BalanceChangeDeleteRequest request);
    Task<BalanceChangeDeleteRequest?> GetLastByUserId(long userId);

    Task DeleteById(Guid id);
}