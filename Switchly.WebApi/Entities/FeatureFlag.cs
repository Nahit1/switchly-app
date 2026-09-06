namespace Switchly.WebApi.Entities;

public class FeatureFlag
{
    public Guid Id { get; set; } = Guid.CreateVersion7();
    public Guid ProjectId { get; set; }
    public FlagsProject FlagsProject { get; set; } = default!;

    public string Key { get; set; } = default!;
    public string Name { get; set; } = default!;
    public string? Description { get; set; }

    public bool IsArchived { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    
    public ICollection<FeatureFlagEnvironment> FeatureFlagEnvironments { get; set; } = new List<FeatureFlagEnvironment>();
}