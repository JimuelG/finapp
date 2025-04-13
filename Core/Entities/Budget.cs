namespace Core.Entities;

public class Budget : BaseEntity
{
    public string Category { get; set; } = string.Empty;
    public decimal Limit { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndData { get; set; }

    public int UserId { get; set; }
    public User User { get; set; }
}
