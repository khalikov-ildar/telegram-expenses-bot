using ExpensesBot.Core.Entities;
using ExpensesBot.Core.Repositories;

namespace ExpensesBot.Infrastructure.Repositories;

public class BalanceChangeDeleteRequestRepository : IBalanceChangeDeleteRequestRepository
{
    private readonly List<BalanceChangeDeleteRequest> _requests = [];

    public async Task SaveChangeDeleteRequest(BalanceChangeDeleteRequest request)
    {
        _requests.Add(request);
    }

    public Task<BalanceChangeDeleteRequest?> GetLastByUserId(long userId)
    {
        return Task.FromResult(_requests.FindLast(r => r.UserId == userId));
    }

    public async Task DeleteById(Guid id)
    {
        var index = _requests.FindIndex((r) => r.Id == id);
        _requests.RemoveAt(index);
    }
}