namespace EelegantIot.Shared.Requests.DeviceList;

public record DeviceItemDto(Guid Id, bool IsOn, string Title, string Pin, TimeOnly? StartOrEndAt);