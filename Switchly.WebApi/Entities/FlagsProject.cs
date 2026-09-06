namespace Switchly.WebApi.Entities;

public class FlagsProject
{
    public Guid Id { get; set; } = Guid.CreateVersion7();
    public Guid OrganizationId { get; set; }
    public Organization Organization { get; set; } = default!;

    public string Name { get; set; } = default!;
    public string Key { get; set; } = default!;
    public string? Description { get; set; }
    public bool IsArchived { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public ICollection<ProjectEnvironment> Environments { get; set; } = new List<ProjectEnvironment>();

    public ICollection<FeatureFlag> FeatureFlags { get; set; } = new List<FeatureFlag>();
}