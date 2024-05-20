using EelegantIot.Shared.Hubs;

namespace EelegantIot.Api.Hubs;

public interface IDeviceUpdateClient
{
    public Task OnDeviceUpdated(DeviceHubUpdateDto deviceItemDto);
}