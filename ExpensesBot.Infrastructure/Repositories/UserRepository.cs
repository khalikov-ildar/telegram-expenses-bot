using ExpensesBot.Core;
using ExpensesBot.Core.Entities;
using ExpensesBot.Core.Repositories;

namespace ExpensesBot.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly List<User> _users = [];
    
    public async Task AddUser(User user)
    {
        _users.Add(user);
    }

    public Task<User?> GetUserById(long id)
    {
        return Task.FromResult(_users.FirstOrDefault(u => u.Id == id));
    }

    public async Task UpdateUser(User user)
    {
        var userIdx = _users.FindIndex(u => u.Id == user.Id);
        _users[userIdx] = user;
    }
}