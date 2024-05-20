namespace EelegantIot.Shared.Hubs;

public record DeviceHubUpdateDto(double Humidity,double Temperature,double Current,double Voltage,bool IsOn);