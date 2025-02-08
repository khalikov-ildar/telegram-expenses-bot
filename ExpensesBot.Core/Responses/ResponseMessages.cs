using ExpensesBot.Core.Enums;

namespace ExpensesBot.Core.Responses;

public static class ResponseMessages
{
    public static string ReportExample(Languages language)
    {
        const string enMessage = "/report 05.12.2020-21.05.2021 | /report 05.12.2020-21.05.2021 Groceries";
        const string ruMessage = "/отчет 05.12.2020-21.05.2021 | /отчет 05.12.2020-21.05.2021 Продукты";
        return language == Languages.En ? enMessage : ruMessage;
    }

    public static string Start(Languages language)
    {
        return language == Languages.En
            ? "To see available commands send the /help command"
            : "Чтобы посмотреть доступные комманды отправьте комманду /помощь";
    }

    public static string Help(Languages language)
    {
        const string ruMessage = """
                                 <b>Доступные комманды:</b>
                                 /прибавить => "/прибавить [Категория] [Сумма]"
                                 /вычесть => "/вычесть [Категория] [Сумма]"
                                 /изменить => "/изменить [Идентификатор записи] [<b>Опционально</b> Категория в формате "категория: [ваша_категория]"] [<b>Опционально</b> Сумма в формате "сумма: [ваша_сумма]"]" Хотя бы одно из опциональных значений должно быть заполнено
                                 /отчет, /отчёт => "/отчет [Дата начала]-[Дата конца] [<b>Опционально</b> Категория для фильтрования изменений баланса]" Формат даты: {ДД:ММ:ГГГГ}. По умолчанию создаёт отчет за последний месяц
                                 /пример-отчета, /пример-отчёта => Показывает пример написания запроса для команды "/отчет"
                                 /помощь => Повторно отправляет данное сообщение
                                 """;
        const string enMessage = """
                                 <b>Available commands:</b>
                                 /add=> "/add [Category] [Amount]"
                                 /subtract => "/subtract [Category] [Amount]"
                                 /edit => "/edit [Id of record] [<b>Optional</b> Category formatted in "category: [your_category]"] [<b>Optional</b> Amount formatted in "amount: [your_amount]"]" At least one optional field must be filled
                                 /report => "/report [Date from]-[Date to] [<b>Optional</b> Category to filter the changes of balance]" Date format: {DD:MM:YYYY}. Generates the report for the last month by default.
                                 /report-example => Shows the example of using the "/report" command
                                 /help => Prints this message again
                                 """;
        return language == Languages.En ? enMessage : ruMessage;
    }

    public static string ChangeRegistered(decimal amount, string category, Languages language)
    {
        return language == Languages.En
            ? $"Change successfully registered: {amount} for category {category}"
            : $"Изменение успешно зарегистрировано: {amount} для категории {category}";
    }

    public static string ChooseOption(Languages language)
    {
        return language == Languages.En ? "Choose the export option" : "Выберите метод получения";
    }

    public static string ApproveNegativeBalance(string balance,Languages language)
    {
        return language == Languages.En
            ? $"Deleting this change will result in negative balance ({balance}), do you want to proceed?"
            : $"Удаление данной записи приведёт к отрицательному балансу ({balance}), хотите ли Вы продолжить?";
    }

    public static string BalanceChangeDeletedSuccessfully(Languages language)
    {
        return language == Languages.En
            ? "Balance change was deleted successfully"
            : "Запись удалена успешно";
    }
}