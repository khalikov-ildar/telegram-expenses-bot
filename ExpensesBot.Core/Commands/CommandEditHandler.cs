using ErrorOr;
using ExpensesBot.Api.Services.Common;
using ExpensesBot.Api.Services.MessageHandler;
using ExpensesBot.Core.Dtos;
using ExpensesBot.Core.Enums;
using ExpensesBot.Core.Errors.Balance;
using ExpensesBot.Core.Errors.User;
using ExpensesBot.Core.Interfaces;
using ExpensesBot.Core.Repositories;
using ExpensesBot.Core.Services;

namespace ExpensesBot.Core.Commands;

public class CommandEditHandler : ICommandHandler<PureEditCommand, IHandlerOutput<string>>
{
    private readonly IContextProvider _contextProvider;
    private readonly IBalanceChangeRepository _changeRepository;
    private readonly IUserRepository _userRepository;

    public CommandEditHandler(IContextProvider contextProvider, IBalanceChangeRepository changeRepository, IUserRepository userRepository)
    {
        _contextProvider = contextProvider;
        _changeRepository = changeRepository;
        _userRepository = userRepository;
    }

    public async Task<IHandlerOutput<string>> Handle(PureEditCommand request)
    {
        var validationResult = ValidateInput(request.Language);

        if (validationResult.IsError)
        {
            return CreateOutput(validationResult.FirstError);
        }

        var (editPayload, userId) = validationResult.Value;
        
        var change = await _changeRepository.GetBalanceChangeById(editPayload.ChangeId);

        if (change is null)
        {
            return CreateOutput(BalanceErrors.BalanceChangeWasNotFound(editPayload.ChangeId, request.Language));
        }

        if (change.UserId != userId)
        {
            return CreateOutput(UserErrors.DontHavePermissionForAction(request.Language));
        }

        change.Edit(editPayload.Category, editPayload.Amount);
        
        var user = await _userRepository.GetUserById(userId);

        var result = user!.ChangeBalance(change);
        
        if (result.IsError)
        {
            return CreateOutput(result.FirstError);
        }

        var msg = $"Success: Id: {editPayload.ChangeId} changed";

        return CreateOutput(msg);
    }

    private ErrorOr<(EditPayload EditPayload, long UserId)> ValidateInput(Languages language)
    {
        var messageText = _contextProvider.GetMessageText();
        
        if (!MessageParser.TryParseEditPayloadFromString(messageText, language, out var editPayload))
        {
            return Error.Validation("", "Invalid format");
        }

        var userId = _contextProvider.GetUserId();

        return userId.IsError ? userId.FirstError : (editPayload, userId.Value);
    }
    
    private static IHandlerOutput<string> CreateOutput(ErrorOr<string> result, bool needsCallback = false)
    {
        return OutputFactory<string>.Create(result, nameof(CommandEditHandler), needsCallback);
    }
}

public record PureEditCommand(Languages Language = Languages.En);