using EelegantIot.Api.Domain.Entities;
using EelegantIot.Api.Infrastructure;
using EelegantIot.Api.Models;
using EelegantIot.Shared.Requests.NewDevice;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EelegantIot.Api.Api;

[ApiController]
[Route("devices")]
[Authorize]
public class DeviceController(AppDbContext dbContext) : ControllerBase
{
    [HttpPost("new")]
    public async Task<IActionResult> NewDevice(NewDeviceRequest newDeviceRequest)
    {
        Guid userId = Guid.Parse(User.Identity!.Name!);
        User user = dbContext.Users.First(x => x.Id == userId);
        if (dbContext.Devices.Include(deviceLoad => deviceLoad.DeviceUsers)
                .FirstOrDefault(x => x.Identifier == newDeviceRequest.Pin) is { } device)
        {
            if (device.DeviceUsers.Any(x => x.UserId == userId))
            {
                return Ok(new ErrorModel("کاربر قبلا دستگاه را ثبت کرده است"));
            }

            device.DeviceUsers.Add(new UserDevices(user, device, newDeviceRequest.Name));
            await dbContext.SaveChangesAsync();
            return NoContent();
        }

        device = new(Guid.NewGuid(), newDeviceRequest.Pin, DateTime.Now);
        device.DeviceUsers = new();
        device.DeviceUsers.Add(new UserDevices(user, device, newDeviceRequest.Name));
        dbContext.Devices.Add(device);
        await dbContext.SaveChangesAsync();
        return NoContent();
    }
}