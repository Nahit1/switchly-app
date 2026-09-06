using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Switchly.WebApi.Entities;

namespace Switchly.WebApi.Context.Configurations;

public class OrganizationMemberConfiguration : IEntityTypeConfiguration<OrganizationMember>
{
    public void Configure(EntityTypeBuilder<OrganizationMember> builder)
    {
        builder.HasKey(x => x.Id);

        builder.HasIndex(x => new { x.OrganizationId, x.UserId })
            .IsUnique();

        builder.Property(x => x.Role)
            .HasConversion<string>()
            .HasMaxLength(32);

        builder.HasOne(x => x.Organization)
            .WithMany(x => x.Members)
            .HasForeignKey(x => x.OrganizationId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.User)
            .WithMany(x => x.OrganizationMembers)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}