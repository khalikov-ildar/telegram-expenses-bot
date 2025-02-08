using ExpensesBot.Api.Services.Common;
using ExpensesBot.Core.Enums;
using ExpensesBot.Core.Interfaces;
using ExpensesBot.Core.Responses;

namespace ExpensesBot.Core.Commands;

public class CommandReportExampleHandler : ICommandHandler<PureReportExampleCommand, IHandlerOutput<string>>
{
    public Task<IHandlerOutput<string>> Handle(PureReportExampleCommand request)
    {
        return Task.FromResult(OutputFactory<string>.Create(ResponseMessages.ReportExample(request.Language), nameof(CommandReportExampleHandler)));
    }
}

public record PureReportExampleCommand(Languages Language = Languages.En);

