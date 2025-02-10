using ErrorOr;
using ExpensesBot.Api.Services.Common;
using ExpensesBot.Core.Enums;
using ExpensesBot.Core.Interfaces;
using ExpensesBot.Core.Repositories;
using ExpensesBot.Core.Responses;
using ExpensesBot.Core.Services;

namespace ExpensesBot.Core.Commands;

public class CommandNegativeApprovalHandler : ICommandHandler<PureNegativeApprovalCommand, IHandlerOutput<string>>
{
    private readonly IContextProvider _contextProvider;
    private readonly IBalanceChangeDeleteRequestRepository _deleteRequestRepository;
    private readonly IBalanceChangeRepository _balanceChangeRepository;
    private readonly IUserRepository _userRepository;

    public CommandNegativeApprovalHandler(IContextProvider contextProvider, IBalanceChangeDeleteRequestRepository deleteRequestRepository, IBalanceChangeRepository balanceChangeRepository, IUserRepository userRepository)
    {
        _contextProvider = contextProvider;
        _deleteRequestRepository = deleteRequestRepository;
        _balanceChangeRepository = balanceChangeRepository;
        _userRepository = userRepository;
    }

    public async Task<IHandlerOutput<string>> Handle(PureNegativeApprovalCommand request)
    {
        _ = Enum.TryParse<NegativeBalanceChangeResponse>(request.Type, out var type);
        
        var userId = _contextProvider.GetUserId();

        if (userId.IsError)
        {
            return CreateOutput(userId.FirstError);
        }
        
        var deleteRequest = await _deleteRequestRepository.GetLastByUserId(userId.Value);

        if (deleteRequest is null)
        {
            return CreateOutput(Error.NotFound("", "Your request was not found"));
        }

        
        if (type == NegativeBalanceChangeResponse.No)
        {
            await _deleteRequestRepository.DeleteById(deleteRequest.Id);
            return CreateOutput("Delete cancelled");
        }

        var change = await _balanceChangeRepository.GetBalanceChangeById(deleteRequest.ChangeId);
        var user = await _userRepository.GetUserById(deleteRequest.UserId);
        
        user!.EditBalance(change!.Amount);

        await _userRepository.UpdateUser(user);
        await _balanceChangeRepository.DeleteBalanceChangeById(deleteRequest.ChangeId);

        return CreateOutput(ResponseMessages.BalanceChangeDeletedWithNewBalance(user.Balance, request.Language));
    }
    
    private static IHandlerOutput<string> CreateOutput(ErrorOr<string> result, bool needsCallback = false)
    {
        return OutputFactory<string>.Create(result, nameof(CommandNegativeApprovalHandler), needsCallback);
    }
}

public record PureNegativeApprovalCommand(string Type, Languages Language = Languages.En);