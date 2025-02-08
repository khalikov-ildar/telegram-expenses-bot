using ExpensesBot.Core.Dtos;
using ExpensesBot.Core.Repositories;

namespace ExpensesBot.Infrastructure.Repositories;

public class BalanceReportRequestRepository : IBalanceReportRequestRepository
{
    private List<BalanceReportRequest> _requests = [];

    public async Task<BalanceReportRequest?> GetLastByUserId(long userId)
    {
        return _requests.FindLast(r => r.UserId == userId);
    }

    public async Task Add(BalanceReportRequest request)
    {
        _requests.Add(request);
    }

    public async Task Delete(Guid id)
    {
        _requests = _requests.FindAll(r => r.Id != id);
    }
}