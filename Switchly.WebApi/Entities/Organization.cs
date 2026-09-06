namespace Switchly.WebApi.Entities;

public class Organization
{
    public Guid Id { get; set; } = Guid.CreateVersion7();
    public string Name { get; set; } = default!;
    public string PublicKey { get; set; } = default!;
    public DateTimeOffset CreatedAt { get; set; }
    
    public ICollection<OrganizationMember> Members { get; set; } = new List<OrganizationMember>();
    public ICollection<FlagsProject> Projects { get; set; } = new List<FlagsProject>();
    public ICollection<SegmentGroup> SegmentGroups { get; set; } = new List<SegmentGroup>();
}