using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Switchly.WebApi.Entities;

namespace Switchly.WebApi.Context.Configurations;

public class FeatureFlagSegmentConfiguration : IEntityTypeConfiguration<FeatureFlagSegment>
{
    public void Configure(EntityTypeBuilder<FeatureFlagSegment> builder)
    {
        builder.HasKey(x => x.Id);

        builder.HasIndex(x => new { x.FeatureFlagEnvironmentId, x.SegmentGroupId })
            .IsUnique();

        builder.HasIndex(x => new { x.FeatureFlagEnvironmentId, x.Priority });

        builder.HasOne(x => x.FeatureFlagEnvironment)
            .WithMany(x => x.FeatureFlagSegments)
            .HasForeignKey(x => x.FeatureFlagEnvironmentId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.SegmentGroup)
            .WithMany(x => x.FeatureFlagSegments)
            .HasForeignKey(x => x.SegmentGroupId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
