using EelegantIot.Api.Domain.Entities;

namespace EelegantIot.Api.Hubs;

public interface IDeviceUpdateNotificationService
{
    public Task DeviceUpdated(Device device);
}