using System.Data;
using ErrorOr;
using ExpensesBot.Core.Commands;
using ExpensesBot.Core.Enums;
using ExpensesBot.Core.Interfaces;
using ExpensesBot.Core.Services;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace ExpensesBot.Api.Services.MessageHandler;

public class MessageCommandDispatcher
{
    private readonly Dictionary<string, Func<Task<IHandlerOutput<string>>>> _handlers;
    private readonly CallbackMapper _callbackMapper;
    private readonly IContextProvider _contextProvider;

    public MessageCommandDispatcher(
        ICommandHandler<PureDeleteBalanceChangeCommand, IHandlerOutput<string>> deleteBalanceChangeHandler,
        ICommandHandler<PureStartCommand, IHandlerOutput<string>> commandStartHandler,
        ICommandHandler<PureBalanceChangeCommand, IHandlerOutput<string>> commandBalanceChangeHandler,
        ICommandHandler<PureEditCommand, IHandlerOutput<string>> editHandler,
        ICommandHandler<PureReportRequestCommand, IHandlerOutput<string>> commandReportRequestHandler,
        ICommandHandler<PureReportExampleCommand, IHandlerOutput<string>> commandReportExampleHandler,
        ICommandHandler<PureHelpCommand, IHandlerOutput<string>> commandHelp,
        CallbackMapper callbackMapper, IContextProvider contextProvider)
    {
        _callbackMapper = callbackMapper;
        _contextProvider = contextProvider;
        _handlers = new Dictionary<string, Func<Task<IHandlerOutput<string>>>>
        {
            { BotCommands.Start, async()  => await commandStartHandler.Handle(new PureStartCommand()) },
            {
                BotCommands.Add,
                async()  =>
                    await commandBalanceChangeHandler.Handle(new PureBalanceChangeCommand(BalanceChangeTypes.Add))
            },
            {
                BotCommands.Subtract,
                async()  =>
                    await commandBalanceChangeHandler.Handle(new PureBalanceChangeCommand(BalanceChangeTypes.Subtract))
            },
            {
                BotCommands.Edit,
                async()  =>
                    await editHandler.Handle(new PureEditCommand())
            },
            {
                BotCommands.Delete,
                async()  =>
                    await deleteBalanceChangeHandler.Handle(new PureDeleteBalanceChangeCommand())
            },
            {
                BotCommands.Report,
                async()  => await commandReportRequestHandler.Handle(new PureReportRequestCommand())
            },
            {
                BotCommands.ReportExample,
                async()  => await commandReportExampleHandler.Handle(new PureReportExampleCommand())
            },
            { BotCommands.Help, async()  => await commandHelp.Handle(new PureHelpCommand()) },
            {
                BotCommands.Прибавить,
                async()  =>
                    await commandBalanceChangeHandler.Handle(new PureBalanceChangeCommand(BalanceChangeTypes.Add,
                        Languages.Ru))
            },
            {
                BotCommands.Вычесть,
                async()  =>
                    await commandBalanceChangeHandler.Handle(new PureBalanceChangeCommand(BalanceChangeTypes.Subtract, Languages.Ru))
            },
            {
                BotCommands.Изменить,
                async()  =>
                    await editHandler.Handle(new PureEditCommand(Languages.Ru))
            },
            {
                BotCommands.Удалить,
                async()  =>
                    await deleteBalanceChangeHandler.Handle(new PureDeleteBalanceChangeCommand(Languages.Ru))
            },
            {
                BotCommands.Отчет,
                async()  =>
                    await commandReportRequestHandler.Handle(new PureReportRequestCommand(Languages.Ru))
            },
            {
                BotCommands.Отчёт,
                async()  =>
                    await commandReportRequestHandler.Handle(new PureReportRequestCommand(Languages.Ru))
            },
            {
                BotCommands.ПримерОтчета,
                async()  =>
                    await commandReportExampleHandler.Handle(new PureReportExampleCommand(Languages.Ru))
            },
            {
                BotCommands.ПримерОтчёта,
                async()  =>
                    await commandReportExampleHandler.Handle(new PureReportExampleCommand(Languages.Ru))
            },
            {
                BotCommands.Помощь,
                async()  => await commandHelp.Handle(new PureHelpCommand(Languages.Ru))
            }
        };
    }
    
    public async Task<ErrorOr<Message>> ProcessMessage(Message message, ITelegramBotClient bot)
    {
        RegisterContext(message);
        var output = await MapHandler(message);

        if (output.Result.IsError)
        {
            return output.Result.FirstError;
        }
        
        if (!output.NeedsCallback)
        {
            return await bot.SendMessage(message.Chat, output.Result.Value, ParseMode.Html);
        }

        var language = _contextProvider.GetUserLanguage(message.From!.Id);
        
        return await _callbackMapper.MapCallback(output.HandlerName, message.Chat, output.Result.Value, language);
    }

    private async Task<IHandlerOutput<string>> MapHandler(Message message)
    {
        var commandName =  MessageParser.ExtractCommand(message.Text!);

        var handlerExists = _handlers.TryGetValue(commandName, out var handler);

        if (!handlerExists)
        {
            throw new NotImplementedException();
        }

        return await handler!.Invoke();
    }

    private void RegisterContext(Message message)
    {
        _contextProvider.RegisterMessageText(message.Text!);
        _contextProvider.RegisterUserId(message.From?.Id);
    }
}