using ExpensesBot.Core.Entities;

namespace ExpensesBot.Core.Repositories;

public interface IUserRepository
{
    Task AddUser(User user);
    Task<User?> GetUserById(long id);

    Task UpdateUser(User user);
}