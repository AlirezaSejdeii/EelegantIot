using EelegantIot.Api.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EelegantIot.Api.Infrastructure.Configurations;

public class UserDevicesConfiguration : IEntityTypeConfiguration<UserDevices>
{
    public void Configure(EntityTypeBuilder<UserDevices> builder)
    {
        builder.HasKey(ud => new { ud.UserId, ud.DeviceId });

        // Configuring relationships
        builder.HasOne(ud => ud.User)
            .WithMany(x => x.UserDevices)
            .HasForeignKey(ud => ud.UserId);

        builder.HasOne(ud => ud.Device)
            .WithMany(x => x.DeviceUsers)
            .HasForeignKey(ud => ud.DeviceId);

        builder.Property(x => x.Name)
            .HasMaxLength(256);
    }
}