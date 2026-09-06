namespace Switchly.WebApi.Entities;

public class ProjectEnvironment
{
    public Guid Id { get; set; } = Guid.CreateVersion7();
    public Guid ProjectId { get; set; }
    public FlagsProject FlagsProject { get; set; } = default!;

    public string Name { get; set; } = default!; // Development, Staging, Production
    public string Key { get; set; } = default!;  // dev, stg, prod
    public bool IsDefault { get; set; }
    public int SortOrder { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    
    public ICollection<FeatureFlagEnvironment> FeatureFlagEnvironments { get; set; } = new List<FeatureFlagEnvironment>();
}