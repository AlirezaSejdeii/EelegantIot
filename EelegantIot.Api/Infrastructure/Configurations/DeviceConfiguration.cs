using EelegantIot.Api.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EelegantIot.Api.Infrastructure.Configurations;

public class DeviceConfiguration : BaseConfiguration<Device>
{
    protected override void ExtraConfigure(EntityTypeBuilder<Device> builder)
    {
        builder.Property(x => x.SettingMode)
            .HasColumnName("setting_mode");
        builder.Property(x => x.Identifier)
            .HasColumnName("identifier")
            .HasMaxLength(64);
        builder.Property(x => x.Humidity)
            .HasColumnName("humidity");
        builder.Property(x => x.Temperature)
            .HasColumnName("temperature");
        builder.Property(x => x.Current)
            .HasColumnName("current");
        builder.Property(x => x.Voltage)
            .HasColumnName("voltage");

        builder.Property(x => x.Saturday)
            .HasColumnName("saturday");
        builder.Property(x => x.Sunday)
            .HasColumnName("sunday");
        builder.Property(x => x.Monday)
            .HasColumnName("monday");
        builder.Property(x => x.Tuesday)
            .HasColumnName("tuesday");
        builder.Property(x => x.Wednesday)
            .HasColumnName("wednesday");
        builder.Property(x => x.Thursday)
            .HasColumnName("thursday");
        builder.Property(x => x.Friday)
            .HasColumnName("friday");
    }
}