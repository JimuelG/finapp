namespace Core.Entities;

public class Income : BaseEntity
{
    public decimal Amount { get; set; }
    public string Source { get; set; } = string.Empty;
    public DateTime Date { get; set; }

    public int UserId { get; set; }
    public User User { get; set; }
}
