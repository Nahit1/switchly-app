using Switchly.WebApi.Models.Enums;

namespace Switchly.WebApi.Entities;

public class SegmentGroup
{
    public Guid Id { get; set; } = Guid.CreateVersion7();
    public Guid OrganizationId { get; set; }
    public Organization Organization { get; set; } = default!;

    public string Name { get; set; } = default!;
    public string Key { get; set; } = default!;
    public string? Description { get; set; }
    public bool IsArchived { get; set; }
    public DateTimeOffset CreatedAt { get; set; }

    public LogicalOperator LogicalOperator { get; set; } = LogicalOperator.And;
    
    public ICollection<SegmentRule> Rules { get; set; } = new List<SegmentRule>();

    public ICollection<FeatureFlagSegment> FeatureFlagSegments { get; set; } = new List<FeatureFlagSegment>();
}