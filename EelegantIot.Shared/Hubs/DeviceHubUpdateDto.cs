namespace EelegantIot.Shared.Hubs;

public record DeviceHubUpdateDto(
    Guid DeviceId,
    double Humidity,
    double Temperature,
    double Current,
    double Voltage,
    bool IsOn);