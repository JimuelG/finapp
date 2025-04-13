namespace Core.Entities;

public class Goal : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public decimal TargetAmount { get; set; }
    public decimal CurrentAmount { get; set; }
    public DateTime TargetDate { get; set; }

    public int UserId { get; set; }
    public User User { get; set; }
}
