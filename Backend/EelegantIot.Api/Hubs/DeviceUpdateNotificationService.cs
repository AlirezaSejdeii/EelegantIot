using EelegantIot.Api.Domain.Entities;
using EelegantIot.Shared.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace EelegantIot.Api.Hubs;

public class DeviceUpdateNotificationService : IDeviceUpdateNotificationService
{
    private readonly IHubContext<UpdateDeviceHub, IDeviceUpdateClient> _hubContext;
    private readonly ILogger<DeviceUpdateNotificationService> _logger;

    public DeviceUpdateNotificationService(
        IHubContext<UpdateDeviceHub, IDeviceUpdateClient> hubContext,
        ILogger<DeviceUpdateNotificationService> logger)
    {
        _hubContext = hubContext;
        _logger = logger;
    }

    public async Task DeviceUpdated(Device device)
    {
        List<Guid> givenDeviceUserIds = device.DeviceUsers.Select(x => x.UserId).ToList();
        foreach (Guid userId in givenDeviceUserIds)
        {
            _logger.LogInformation("sending notification to given device user");
            await _hubContext.Clients.User(userId.ToString()).OnDeviceUpdated(new DeviceHubUpdateDto(
                device.Id,
                device.Humidity,
                device.Temperature,
                device.Current,
                device.Voltage,
                device.IsOn));
        }
    }
}