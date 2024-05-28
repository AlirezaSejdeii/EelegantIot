namespace EelegantIot.Shared.Requests.DeviceDetails;

public class DeviceDetailsDto(
    Guid id,
    double current,
    double voltage,
    double temperature,
    double humidity,
    SettingMode settingMode,
    TimeOnly startAt,
    TimeOnly endAt,
    int[]? workingDays)
{
    public Guid Id { get; set; } = id;
    public double Current { set; get; } = current;
    public double Voltage { set; get; } = voltage;
    public double Temperature { set; get; } = temperature;
    public double Humidity { set; get; } = humidity;
    public SettingMode SettingMode { set; get; } = settingMode;
    public TimeOnly StartAt { set; get; } = startAt;
    public TimeOnly EndAt { set; get; } = endAt;
    public int[]? WorkingDays { set; get; } = workingDays;
};