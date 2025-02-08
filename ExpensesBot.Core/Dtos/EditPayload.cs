namespace ExpensesBot.Core.Dtos;

public record EditPayload
{ 
    public Guid ChangeId { get; set; }
    public string? Category { get; set; }
    public decimal? Amount { get; set; }

    public EditPayload(Guid id, string? category, decimal? amount)
    {
        ChangeId = id;
        Category = category;
        Amount = amount;
    }
}