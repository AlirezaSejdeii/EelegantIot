namespace EelegantIot.Api.Domain.Entities;

public class DeviceLog : BaseEntity
{
    public DeviceLog(
        Guid id,
        double humidity,
        double temperature,
        double current,
        double voltage,
        Device device,
        DateTime createdAt)
    {
        Id = id;
        Humidity = humidity;
        Temperature = temperature;
        Current = current;
        Voltage = voltage;
        Device = device;
        CreatedAt = createdAt;
    }

    protected DeviceLog()
    {
    }

    public double Humidity { get; private set; }
    public double Temperature { get; private set; }
    public double Current { get; private set; }
    public double Voltage { get; private set; }
    public Device Device { get; set; }
}