using ErrorOr;
using ExpensesBot.Core.Enums;
using ExpensesBot.Core.Errors.Balance;

namespace ExpensesBot.Core.Entities;

public class User
{
    public long Id { get; private set; }

    public decimal Balance { get; private set; }
    public User(long id)
    {
        Id = id;
        Balance = 0;
    }

    public ErrorOr<Success> ChangeBalance(BalanceChange change)
    {
        if (change.Type == BalanceChangeTypes.Add)
        {
            Balance += change.Amount;
            return Result.Success;
        }
        if (Balance < change.Amount)
        {
            return BalanceErrors.BalanceMustBeGreaterThanChangeAmount(Balance, change.Amount);
        }

        Balance -= change.Amount;
        return Result.Success;
    }

    public ErrorOr<Success> RevertBalanceChange(BalanceChange change)
    {
        if (change.Type == BalanceChangeTypes.Add)
        {
            if (Balance - change.Amount < 0)
            {
                var negativeBalance = Balance - change.Amount;
                return Error.Validation($"{negativeBalance}", "Deleting balance change will result in negative balance");
            }

            Balance -= change.Amount;
            return Result.Success;
        }

        Balance += change.Amount;
        return Result.Success;
    }

    public void EditBalance(decimal amount)
    {
        Balance -= amount;
    }
}