using ExpensesBot.Api.Services.Common;
using ExpensesBot.Core.Enums;
using ExpensesBot.Core.Interfaces;
using ExpensesBot.Core.Responses;

namespace ExpensesBot.Core.Commands;

public class CommandHelpHandler : ICommandHandler<PureHelpCommand, IHandlerOutput<string>>
{
    public async Task<IHandlerOutput<string>> Handle(PureHelpCommand request)
    {
        return await Task.FromResult(OutputFactory<string>.Create(ResponseMessages.Help(request.Language), nameof(CommandHelpHandler)));
    }
}

public record PureHelpCommand(Languages Language = Languages.En);