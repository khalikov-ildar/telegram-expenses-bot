using ExpensesBot.Core.Dtos;

namespace ExpensesBot.Core.Repositories;

public interface IBalanceReportRequestRepository
{
    Task<BalanceReportRequest?> GetLastByUserId(long userId);
    Task Add(BalanceReportRequest request);
    Task Delete(Guid id);
}