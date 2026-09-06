using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Switchly.WebApi.Entities;

namespace Switchly.WebApi.Context.Configurations;

public class SegmentRuleConfiguration : IEntityTypeConfiguration<SegmentRule>
{
    public void Configure(EntityTypeBuilder<SegmentRule> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.TraitKey)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.Operator)
            .IsRequired()
            .HasMaxLength(32);

        builder.Property(x => x.Value)
            .IsRequired()
            .HasMaxLength(1000);

        builder.Property(x => x.ValueType)
            .HasConversion<string>()
            .HasMaxLength(32);

        builder.HasIndex(x => new { x.SegmentGroupId, x.SortOrder });

        builder.HasOne(x => x.SegmentGroup)
            .WithMany(x => x.Rules)
            .HasForeignKey(x => x.SegmentGroupId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
