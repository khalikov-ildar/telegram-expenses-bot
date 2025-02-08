using ErrorOr;
using ExpensesBot.Api.Services.Common;
using ExpensesBot.Core.Enums;
using ExpensesBot.Core.Interfaces;
using ExpensesBot.Core.Repositories;
using ExpensesBot.Core.Services;

namespace ExpensesBot.Core.Commands;

public class CommandExportReportHandler : ICommandHandler<PureExportReportCommand, IHandlerOutput<Stream>>
{
    private readonly IContextProvider _contextProvider;
    private readonly IBalanceReportRequestRepository _reportRequestRepository;
    private readonly IBalanceChangeRepository _changeRepository;
    private readonly IReportDataExporter _reportDataExporter;

    public CommandExportReportHandler(IContextProvider contextProvider,IReportDataExporter reportDataExporter, IBalanceReportRequestRepository reportRequestRepository, IBalanceChangeRepository changeRepository)
    {
        _contextProvider = contextProvider;
        _reportDataExporter = reportDataExporter;
        _reportRequestRepository = reportRequestRepository;
        _changeRepository = changeRepository;
    }

    public async Task<IHandlerOutput<Stream>> Handle(PureExportReportCommand request)
    {
        var _ = Enum.TryParse<ReportExportTypes>(request.Type, out var type);
        
        var userId = _contextProvider.GetUserId();

        if (userId.IsError)
        {
            return CreateOutput(userId.FirstError);
        }
        
        var reportRequest = await _reportRequestRepository.GetLastByUserId(userId.Value);
        
        var balanceChanges = await _changeRepository.ListAllInDateRange(reportRequest!.RequestedPeriod, reportRequest.Category);

        var report = BalanceReport.Create(reportRequest.UserId, reportRequest.RequestedPeriod,
            reportRequest.Category, balanceChanges);

        return CreateOutput(await _reportDataExporter.Generate(report, type));
    }
    
    private static IHandlerOutput<Stream> CreateOutput(ErrorOr<Stream> result)
    {
        return OutputFactory<Stream>.Create(result, nameof(CommandExportReportHandler));
    }
}

public record PureExportReportCommand(string Type, Languages Language = Languages.En);