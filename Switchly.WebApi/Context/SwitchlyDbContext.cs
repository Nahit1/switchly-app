using Microsoft.EntityFrameworkCore;
using Switchly.WebApi.Entities;

namespace Switchly.WebApi.Context;

public class SwitchlyDbContext(DbContextOptions<SwitchlyDbContext> opts):DbContext(opts)
{
    public DbSet<User> Users => Set<User>();
    public DbSet<Organization> Organizations => Set<Organization>();
    public DbSet<OrganizationMember> OrganizationMembers => Set<OrganizationMember>();
    public DbSet<FlagsProject> FlagsProjects => Set<FlagsProject>();
    public DbSet<ProjectEnvironment> ProjectEnvironments => Set<ProjectEnvironment>();
    public DbSet<FeatureFlag> FeatureFlags => Set<FeatureFlag>();
    public DbSet<FeatureFlagEnvironment> FeatureFlagEnvironments => Set<FeatureFlagEnvironment>();
    public DbSet<FeatureFlagSegment> FeatureFlagSegments => Set<FeatureFlagSegment>();
    public DbSet<SegmentGroup> SegmentGroups => Set<SegmentGroup>();
    public DbSet<SegmentRule> SegmentRules => Set<SegmentRule>();
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(SwitchlyDbContext).Assembly);
    }
}