using ErrorOr;
using ExpensesBot.Core.Commands;
using ExpensesBot.Core.Enums;
using ExpensesBot.Core.Interfaces;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace ExpensesBot.Api.Services.CallbackHandler;

public class CallbackCommandDispatcher
{
    private readonly ITelegramBotClient _bot;
    private readonly Dictionary<Func<string, bool>, Func<string, CallbackQuery, Task<ErrorOr<Message>>>> _handlers = new();

    public CallbackCommandDispatcher(
        ICommandHandler<PureExportReportCommand, IHandlerOutput<Stream>> exportReportHandler,
        ICommandHandler<PureNegativeApprovalCommand, IHandlerOutput<string>> negativeBalanceApprovalHandler, ITelegramBotClient bot)
    {
        _bot = bot;
        _handlers.Add(
            data => Enum.TryParse<ReportExportTypes>(data, out var _),
            async (data, query) => await HandleStream(exportReportHandler, data, query));

        _handlers.Add(
            data => Enum.TryParse<NegativeBalanceChangeResponse>(data, out var _),
            async (data, query) => await HandleString(negativeBalanceApprovalHandler, data, query));
    }

    public async Task<ErrorOr<Message>> ProcessCallbackQuery(CallbackQuery query)
    {
        var data = query.Data!;

        foreach (var (predicate, handler) in _handlers)
        {
            if (predicate(data))
            {
                return await handler(data, query);
            }
        }

        return await _bot.SendMessage(query.Message!.Chat, "Oops");
    }

    private async Task<ErrorOr<Message>> HandleString(ICommandHandler<PureNegativeApprovalCommand, IHandlerOutput<string>> handler, string data, CallbackQuery query)
    {
        var response = await handler.Handle(new PureNegativeApprovalCommand(data));
        return await ProcessResponse(response, data, query);
    }

    private async Task<ErrorOr<Message>> HandleStream(ICommandHandler<PureExportReportCommand, IHandlerOutput<Stream>> handler, string data, CallbackQuery query)
    {
        var response = await handler.Handle(new PureExportReportCommand(data));
        return await ProcessResponse(response, data, query);
    }


    private async Task<ErrorOr<Message>> ProcessResponse<T>(IHandlerOutput<T> response, string data, CallbackQuery query)
    {
        await ConfirmCallbackAndDeleteMessage(query);
        
        if (response.Result.IsError)
        {
            return response.Result.FirstError;
        }

        if (response.Result.Value is Stream stream)
        {

            _ = Enum.TryParse<ReportExportTypes>(data, out var type);

            if (type == ReportExportTypes.Text)
            {
                using var reader = new StreamReader(stream);
                var textMessage = await reader.ReadToEndAsync();
                return await _bot.SendMessage(query.Message!.Chat, textMessage);
            }

            var inputFile = InputFile.FromStream(stream);
            return await _bot.SendDocument(query.Message!.Chat, inputFile); 
        }
        if (response.Result.Value is string str)
        {
            return await _bot.SendMessage(query.Message!.Chat, str); 
        }

        return Error.NotFound("", "Unsupported response type");
    }

    private async Task ConfirmCallbackAndDeleteMessage(CallbackQuery query)
    {
        await _bot.AnswerCallbackQuery(query.Id);
        await _bot.DeleteMessage(query.Message!.Chat.Id, query.Message!.Id);
    }
}