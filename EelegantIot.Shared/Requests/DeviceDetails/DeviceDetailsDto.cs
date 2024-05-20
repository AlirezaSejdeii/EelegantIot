namespace EelegantIot.Shared.Requests.DeviceDetails;

public record DeviceDetailsDto(
    double Current,
    double Voltage,
    double Temperature,
    double Humidity,
    SettingMode SettingMode,
    DateOnly StartAt,
    DateOnly EndAt,
    int[]? WorkingDays);