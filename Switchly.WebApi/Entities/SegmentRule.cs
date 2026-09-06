using Switchly.WebApi.Models.Enums;

namespace Switchly.WebApi.Entities;

public class SegmentRule
{
    public Guid Id { get; set; } = Guid.CreateVersion7();
    public Guid SegmentGroupId { get; set; }
    public SegmentGroup SegmentGroup { get; set; } = default!;
    public string TraitKey { get; set; } = default!;
    public string Operator { get; set; } = default!;
    public string Value { get; set; } = default!;
    public SegmentValueType ValueType { get; set; }
    public int SortOrder { get; set; }
}