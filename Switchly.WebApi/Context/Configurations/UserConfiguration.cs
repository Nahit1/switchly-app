using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Switchly.WebApi.Entities;

namespace Switchly.WebApi.Context.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Email)
            .IsRequired()
            .HasMaxLength(320);

        builder.Property(x => x.PasswordHash)
            .IsRequired()
            .HasMaxLength(1000);

        builder.HasIndex(x => x.Email)
            .IsUnique();
    }
}