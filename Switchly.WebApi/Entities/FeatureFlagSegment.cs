using Switchly.WebApi.Models.Enums;

namespace Switchly.WebApi.Entities;

public class FeatureFlagSegment
{
    public Guid Id { get; set; } = Guid.CreateVersion7();

    public Guid FeatureFlagEnvironmentId { get; set; }
    public FeatureFlagEnvironment FeatureFlagEnvironment { get; set; } = default!;

    public Guid SegmentGroupId { get; set; }
    public SegmentGroup SegmentGroup { get; set; } = default!;
    
    public int Priority { get; set; }      
    public DateTimeOffset CreatedAt { get; set; }
}