using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Switchly.WebApi.Entities;

namespace Switchly.WebApi.Context.Configurations;

public class FlagsProjectConfiguration : IEntityTypeConfiguration<FlagsProject>
{
    public void Configure(EntityTypeBuilder<FlagsProject> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(x => x.Key)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.Description)
            .HasMaxLength(1000);

        builder.HasIndex(x => new { x.OrganizationId, x.Key })
            .IsUnique();

        builder.HasOne(x => x.Organization)
            .WithMany(x => x.Projects)
            .HasForeignKey(x => x.OrganizationId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
