using Core.Entities;
using Infrastructure.Data.Config;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class AppDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<Budget> Budgets { get; set; }
    public DbSet<Expense> Expenses { get; set; }
    public DbSet<FinancialTip> FinancialTips { get; set; }
    public DbSet<Goal> Goals { get; set; }
    public DbSet<Income> Incomes { get; set; }
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Model.GetEntityTypes().ToList()
            .ForEach(e => e.SetTableName(e.GetTableName()?.ToLower()));
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(BudgetConfiguration).Assembly);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ExpenseConfiguration).Assembly);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(GoalConfiguration).Assembly);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(IncomeConfiguration).Assembly);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {   
        foreach (var entry in ChangeTracker.Entries<AuditableEntity>())
        {
            if(entry.State == EntityState.Added)
            {
                entry.Entity.CreatedAt = DateTime.UtcNow;
            }
            else if (entry.State == EntityState.Modified)
            {
                entry.Entity.UpdatedAt = DateTime.UtcNow;
            }
        }

        return await base.SaveChangesAsync(cancellationToken);
    }
}