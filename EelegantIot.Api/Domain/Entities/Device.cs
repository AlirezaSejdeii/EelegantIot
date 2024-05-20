using EelegantIot.Api.Domain.Enums;

namespace EelegantIot.Api.Domain.Entities;

public class Device : BaseEntity
{
    public Device(Guid id, string identifier, DateTime createdAt)
    {
        Id = id;
        Identifier = identifier;
        Humidity = 0;
        Temperature = 0;
        Current = 0;
        Voltage = 0;
        SettingMode = SettingMode.Manual;
        CreatedAt = createdAt;
    }

    protected Device()
    {
    }

    public SettingMode SettingMode { get; private set; }
    public string Identifier { get; private set; }
    public double Humidity { get; private set; }
    public double Temperature { get; private set; }
    public double Current { get; private set; }
    public double Voltage { get; private set; }
    public bool IsOn { get; set; }

    public bool Saturday { get; private set; }
    public bool Sunday { get; private set; }
    public bool Monday { get; private set; }
    public bool Tuesday { get; private set; }
    public bool Wednesday { get; private set; }
    public bool Thursday { get; private set; }
    public bool Friday { get; private set; }
    public List<UserDevices> DeviceUsers { get; set; }
    public List<DeviceLog> Logs { get; set; }
}