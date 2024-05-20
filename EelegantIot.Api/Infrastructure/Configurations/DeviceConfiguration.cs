using System.Text.Json;
using EelegantIot.Api.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EelegantIot.Api.Infrastructure.Configurations;

public class DeviceConfiguration : BaseConfiguration<Device>
{
    protected override void ExtraConfigure(EntityTypeBuilder<Device> builder)
    {
        builder.ToTable("device");
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
        builder.Property(x => x.IsOn)
            .HasColumnName("is_on");
        builder.Property(x => x.StartAt)
            .HasColumnName("start_at");
        builder.Property(x => x.EndAt)
            .HasColumnName("end_at");

        builder.Property(d => d.DayOfWeeks)
            .HasConversion(
                v => JsonSerializer.Serialize(v, (JsonSerializerOptions)null!),
                v => JsonSerializer.Deserialize<int[]>(v, (JsonSerializerOptions)null!))
            .HasColumnType("nvarchar(max)");
    }
}