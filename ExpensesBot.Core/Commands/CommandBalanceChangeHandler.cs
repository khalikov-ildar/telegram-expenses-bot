using ErrorOr;
using ExpensesBot.Api.Services.Common;
using ExpensesBot.Api.Services.MessageHandler;
using ExpensesBot.Core.Entities;
using ExpensesBot.Core.Enums;
using ExpensesBot.Core.Errors.Amount;
using ExpensesBot.Core.Errors.User;
using ExpensesBot.Core.Interfaces;
using ExpensesBot.Core.Repositories;
using ExpensesBot.Core.Responses;
using ExpensesBot.Core.Services;

namespace ExpensesBot.Core.Commands;

public class CommandBalanceChangeHandler : ICommandHandler<PureBalanceChangeCommand, IHandlerOutput<string>>
{
    private readonly IContextProvider _contextProvider;
    private readonly IUserRepository _userRepository;
    private readonly IBalanceChangeRepository _changeRepository;

    public CommandBalanceChangeHandler(IContextProvider contextProvider, IUserRepository userRepository, IBalanceChangeRepository changeRepository)
    {
        _contextProvider = contextProvider;
        _userRepository = userRepository;
        _changeRepository = changeRepository;
    }

    public async Task<IHandlerOutput<string>> Handle(PureBalanceChangeCommand request)
    {
        var validationResult = ValidateInputs(request.Language);

        if (validationResult.IsError)
        {
            return CreateOutput(validationResult.FirstError);
        }

        var (category, amount, userId) = validationResult.Value;

        var balanceChange = new BalanceChange(userId, request.BalanceChangeType, category.ToLower(), amount);

        var user = await _userRepository.GetUserById(userId);

        if (user is null)
        {
            return CreateOutput(UserErrors.UserIsNotInitialized(request.Language));
        }
        
        var changeResult = user.ChangeBalance(balanceChange);

        if (changeResult.IsError)
        {
            return CreateOutput(changeResult.FirstError);
        }
        
        await _changeRepository.AddBalanceChange(balanceChange);
        
        await _userRepository.UpdateUser(user);
        

        var response = ResponseMessages.ChangeRegistered(amount, category, request.Language);

        return CreateOutput(response);
    }

    private ErrorOr<(string Category, decimal Amount, long UserId)> ValidateInputs(Languages language)
    {
        var extractingResult = MessageParser.TryParseCategoryWithChangeAmountFromString(_contextProvider.GetMessageText(), out var result);
        if (!extractingResult)
        {
            return AmountErrors.AmountIsNotANumber(language);
        }

        if (result.Amount <= 0)
        {
            return AmountErrors.AmountIsNegativeOrZero(language);
        }

        var userId = _contextProvider.GetUserId();

        if (userId.IsError)
        {
            return userId.FirstError;
        }

        return (result.Category, result.Amount, userId.Value);
    }
    
    private static IHandlerOutput<string> CreateOutput(ErrorOr<string> result, bool needsCallback = false)
    {
        return OutputFactory<string>.Create(result, nameof(CommandBalanceChangeHandler), needsCallback);
    }
}

public record PureBalanceChangeCommand(BalanceChangeTypes BalanceChangeType, Languages Language = Languages.En);