using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Switchly.WebApi.Entities;

namespace Switchly.WebApi.Context.Configurations;

public class ProjectEnvironmentConfiguration : IEntityTypeConfiguration<ProjectEnvironment>
{
    public void Configure(EntityTypeBuilder<ProjectEnvironment> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(x => x.Key)
            .IsRequired()
            .HasMaxLength(100);

        builder.HasIndex(x => new { x.ProjectId, x.Key })
            .IsUnique();

        builder.HasOne(x => x.FlagsProject)
            .WithMany(x => x.Environments)
            .HasForeignKey(x => x.ProjectId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
