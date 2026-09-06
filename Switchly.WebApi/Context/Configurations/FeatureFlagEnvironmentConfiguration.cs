using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Switchly.WebApi.Entities;

namespace Switchly.WebApi.Context.Configurations;

public class FeatureFlagEnvironmentConfiguration : IEntityTypeConfiguration<FeatureFlagEnvironment>
{
    public void Configure(EntityTypeBuilder<FeatureFlagEnvironment> builder)
    {
        builder.HasKey(x => x.Id);

        builder.HasIndex(x => new { x.FeatureFlagId, x.ProjectEnvironmentId })
            .IsUnique();

        builder.Property(x => x.DefaultRolloutKind)
            .HasConversion<string>()
            .HasMaxLength(32);

        builder.ToTable(t =>
            t.HasCheckConstraint(
                "CK_FeatureFlagEnvironment_DefaultRolloutPercentage",
                "\"DefaultRolloutPercentage\" >= 0 AND \"DefaultRolloutPercentage\" <= 100"));

        builder.HasOne(x => x.FeatureFlag)
            .WithMany(x => x.FeatureFlagEnvironments)
            .HasForeignKey(x => x.FeatureFlagId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.ProjectEnvironment)
            .WithMany(x => x.FeatureFlagEnvironments)
            .HasForeignKey(x => x.ProjectEnvironmentId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}