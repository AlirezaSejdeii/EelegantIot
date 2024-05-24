namespace EelegantIot.Shared.Requests.DeviceList;

public record DeviceItemDto(bool IsOn, string Title, string Pin, TimeOnly? StartOrOffAt);