using EelegantIot.Api.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EelegantIot.Api.Infrastructure.Configurations;

public class UserConfiguration:BaseConfiguration<User>
{
    protected override void ExtraConfigure(EntityTypeBuilder<User> builder)
    {
        builder.Property(x => x.Username)
            .HasColumnName("username")
            .HasMaxLength(64);
        builder.Property(x => x.Password)
            .HasColumnName("password")
            .HasMaxLength(64);
        builder.HasMany(x => x.Devices)
            .WithMany(x => x.Users);
    }
}