using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Config;

public class GoalConfiguration : IEntityTypeConfiguration<Goal>
{
    public void Configure(EntityTypeBuilder<Goal> builder)
    {
        builder.Property(x => x.TargetAmount).HasColumnType("decimal(18,2)").IsRequired();
        builder.Property(x => x.CurrentAmount).HasColumnType("decimal(18,2)").IsRequired();
    }
}
