namespace Core.Entities;

public class Expense : BaseEntity
{
    public decimal Amount { get; set; }
    public string Category { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public string Description { get; set; } = string.Empty;

    public int UserId { get; set; }
    public User User { get; set; }
}
