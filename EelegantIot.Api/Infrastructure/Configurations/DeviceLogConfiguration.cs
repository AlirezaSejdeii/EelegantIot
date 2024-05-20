using EelegantIot.Api.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EelegantIot.Api.Infrastructure.Configurations;

public class DeviceLogConfiguration:BaseConfiguration<DeviceLog>
{
    protected override void ExtraConfigure(EntityTypeBuilder<DeviceLog> builder)
    {
        builder.ToTable("device_log");

        builder.Property(x => x.Temperature).HasColumnName("temperature");
        builder.Property(x => x.Humidity).HasColumnName("humidity");
        builder.Property(x => x.Current).HasColumnName("current");
        builder.Property(x => x.Voltage).HasColumnName("voltage");

        builder.HasOne(x => x.Device)
            .WithMany(x => x.Logs);
    }
}