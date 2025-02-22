using ErrorOr;
using ExpensesBot.Api.Services.Common;
using ExpensesBot.Api.Services.MessageHandler;
using ExpensesBot.Core.Entities;
using ExpensesBot.Core.Enums;
using ExpensesBot.Core.Errors.Balance;
using ExpensesBot.Core.Errors.User;
using ExpensesBot.Core.Interfaces;
using ExpensesBot.Core.Repositories;
using ExpensesBot.Core.Responses;
using ExpensesBot.Core.Services;

namespace ExpensesBot.Core.Commands;

public class CommandDeleteBalanceHandler : ICommandHandler<PureDeleteBalanceChangeCommand, IHandlerOutput<string>>
{
    private readonly IContextProvider _contextProvider;
    private readonly IBalanceChangeRepository _changeRepository;
    private readonly IUserRepository _userRepository;
    private readonly IBalanceChangeDeleteRequestRepository _deleteRequestRepository;

    public CommandDeleteBalanceHandler(IContextProvider contextProvider, IBalanceChangeRepository changeRepository, IUserRepository userRepository, IBalanceChangeDeleteRequestRepository deleteRequestRepository)
    {
        _contextProvider = contextProvider;
        _changeRepository = changeRepository;
        _userRepository = userRepository;
        _deleteRequestRepository = deleteRequestRepository;
    }

    public async Task<IHandlerOutput<string>> Handle(PureDeleteBalanceChangeCommand request)
    {
        
        
        var parsedSuccessfully = MessageParser.TryParseChangeIdForDeleteFromString(
            _contextProvider.GetMessageText(), out var id);

        if (!parsedSuccessfully)
        {
            return CreateOutput(BalanceErrors.BalanceChangeIdIsNotValid(request.Language));
        }

        var userId = _contextProvider.GetUserId();

        if (userId.IsError)
        {
            return CreateOutput(UserErrors.UserIsNotInitialized(request.Language));
        }
        
        _contextProvider.RegisterUserLanguage(userId.Value, request.Language);
        
        var change = await _changeRepository.GetBalanceChangeById(id);
        if (change is null)
        {
             return CreateOutput(BalanceErrors.BalanceChangeWasNotFound(id));
        }

        if (userId != change.UserId)
        {
            return CreateOutput(UserErrors.DontHavePermissionForAction(request.Language));
        }

        var user = await _userRepository.GetUserById(userId.Value);

        if (user is null)
        {
            return CreateOutput(UserErrors.UserIsNotInitialized(request.Language));
        }

        var changingBalanceResult = user.RevertBalanceChange(change);

        if (!changingBalanceResult.IsError)
        {
            await _userRepository.UpdateUser(user);
            await _changeRepository.DeleteBalanceChangeById(id);
            return CreateOutput(ResponseMessages.BalanceChangeDeletedWithNewBalance(user.Balance,request.Language));
        }

        
        var deleteRequest = new BalanceChangeDeleteRequest(userId.Value, change.Id);
        await _deleteRequestRepository.SaveChangeDeleteRequest(deleteRequest);

        return CreateOutput(
            ResponseMessages.ApproveNegativeBalance(changingBalanceResult.FirstError.Code, request.Language), true);

    }

    private static IHandlerOutput<string> CreateOutput(ErrorOr<string> result, bool needsCallback = false)
    {
        return OutputFactory<string>.Create(result, nameof(CommandDeleteBalanceHandler), needsCallback);
    }
}

public record PureDeleteBalanceChangeCommand(Languages Language = Languages.En);