using ErrorOr;
using ExpensesBot.Api.Services.Common;
using ExpensesBot.Core.Entities;
using ExpensesBot.Core.Enums;
using ExpensesBot.Core.Interfaces;
using ExpensesBot.Core.Repositories;
using ExpensesBot.Core.Responses;
using ExpensesBot.Core.Services;

namespace ExpensesBot.Core.Commands;

public class CommandStartHandler : ICommandHandler<PureStartCommand, IHandlerOutput<string>>
{
    private readonly IContextProvider _contextProvider;
    private readonly IUserRepository _userRepository;

    public CommandStartHandler(IContextProvider contextProvider, IUserRepository userRepository)
    {
        _contextProvider = contextProvider;
        _userRepository = userRepository;
    }

    public async Task<IHandlerOutput<string>> Handle(PureStartCommand request)
    {
        var userId = _contextProvider.GetUserId();

        if (userId.IsError)
        {
            CreateOutput(userId.FirstError);
        }

        var user = await _userRepository.GetUserById(userId.Value);

        var output = CreateOutput(ResponseMessages.Start(Languages.En));
        
        if (user is not null)
        {
            return output;
        }

        var newUser = new User(userId.Value);

        await _userRepository.AddUser(newUser);

        return output;
    }

    private static IHandlerOutput<string> CreateOutput(ErrorOr<string> result, bool needsCallback = false)
    {
        return OutputFactory<string>.Create(result, nameof(CommandStartHandler), needsCallback);
    }
}

public record PureStartCommand();