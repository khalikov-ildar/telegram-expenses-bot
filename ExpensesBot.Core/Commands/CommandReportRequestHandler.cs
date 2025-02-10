using ErrorOr;
using ExpensesBot.Api.Services.Common;
using ExpensesBot.Api.Services.MessageHandler;
using ExpensesBot.Core.Dtos;
using ExpensesBot.Core.Enums;
using ExpensesBot.Core.Interfaces;
using ExpensesBot.Core.Repositories;
using ExpensesBot.Core.Responses;
using ExpensesBot.Core.Services;

namespace ExpensesBot.Core.Commands;

public class CommandReportRequestHandler : ICommandHandler<PureReportRequestCommand, IHandlerOutput<string>>
{
    private readonly IContextProvider _contextProvider;
    private readonly IBalanceReportRequestRepository _reportRequestRepository;

    public CommandReportRequestHandler(IContextProvider contextProvider, IBalanceReportRequestRepository reportRequestRepository)
    {
        _contextProvider = contextProvider;
        _reportRequestRepository = reportRequestRepository;
    }

    public async Task<IHandlerOutput<string>> Handle(PureReportRequestCommand request)
    {
        var validationResult = ValidateInput();

        if (validationResult.IsError)
        {
            return CreateOutput(validationResult.FirstError);
        }
        

        var (dateRange, category, userId) = validationResult.Value;

        
        _contextProvider.RegisterUserLanguage(userId, request.Language);
        
        var balanceReportRequest = new BalanceReportRequest(userId, dateRange, category);

        await _reportRequestRepository.Add(balanceReportRequest);

        return CreateOutput(ResponseMessages.ChooseOption(request.Language), true);
    }

    private ErrorOr<(DateRange DateRange, string? Category, long UserId)> ValidateInput(
        )
    {
        var messageText = _contextProvider.GetMessageText();
        
        var parsedSuccessfully =
            MessageParser.TryParseDateRangeWithCategoryFromString(messageText, out var result);

        if (!parsedSuccessfully)
        {
            return Error.Validation("", "Wrong format");
        }

        var userId = _contextProvider.GetUserId();

        return userId.IsError ? userId.FirstError : (result.DateRange, result.Category, userId.Value);
    }

    private static IHandlerOutput<string> CreateOutput(ErrorOr<string> result, bool needsCallback = false)
    {
        return OutputFactory<string>.Create(result, nameof(CommandReportRequestHandler), needsCallback);
    }
}


    

public record PureReportRequestCommand(Languages Language = Languages.En);