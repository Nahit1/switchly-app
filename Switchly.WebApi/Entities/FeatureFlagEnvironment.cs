using Switchly.WebApi.Models.Enums;

namespace Switchly.WebApi.Entities;

public class FeatureFlagEnvironment
{
    public Guid Id { get; set; } = Guid.CreateVersion7();

    public Guid FeatureFlagId { get; set; }
    public FeatureFlag FeatureFlag { get; set; } = default!;

    public Guid ProjectEnvironmentId { get; set; }
    public ProjectEnvironment ProjectEnvironment { get; set; } = default!;

    public bool IsEnabled { get; set; }
    public RolloutKind DefaultRolloutKind { get; set; }
    public int DefaultRolloutPercentage { get; set; }  // 0–100

    public DateTimeOffset UpdatedAt { get; set; }
    public ICollection<FeatureFlagSegment> FeatureFlagSegments { get; set; } = new List<FeatureFlagSegment>();
}