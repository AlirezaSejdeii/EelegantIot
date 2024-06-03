using System.Text.Json;
using EelegantIot.Api.Domain.Entities;
using EelegantIot.Api.Hubs;
using EelegantIot.Api.Infrastructure;
using EelegantIot.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EelegantIot.Api.Api;

[ApiController]
[Route("device-manager")]
public class DeviceManagerController : ControllerBase
{
    private readonly AppDbContext _dbContext;
    private readonly ILogger<DeviceManagerController> _logger;
    private readonly IDeviceUpdateNotificationService _notificationService;

    public DeviceManagerController(AppDbContext dbContext, ILogger<DeviceManagerController> logger,
        IDeviceUpdateNotificationService notificationService)
    {
        _dbContext = dbContext;
        _logger = logger;
        _notificationService = notificationService;
    }

    [HttpGet("rely-status/{id}")]
    public async Task<ActionResult<GetRelyStatus>> GetRelyStatus(string id)
    {
        _logger.LogInformation("Getting relay status");
        _logger.LogInformation("Fetching device");
        Device? device = await _dbContext.Devices.FirstOrDefaultAsync(x => x.Identifier == id);
        if (device is null)
        {
            _logger.LogInformation("Device were not found");
            return NotFound("Device could not be found");
        }

        return Ok(new GetRelyStatus(device.IsOn));
    }

    [HttpPut("update-values/{id}")]
    public async Task<IActionResult> UpdateValues(string id, UpdateDeviceValues values)
    {
        Console.WriteLine(JsonSerializer.Serialize(values));
        _logger.LogInformation("Updating device values");
        _logger.LogInformation("Fetching device");
        Device? device = await _dbContext.Devices.Include(x => x.Logs).Include(x=>x.DeviceUsers).FirstOrDefaultAsync(x => x.Identifier == id);
        if (device is null)
        {
            _logger.LogInformation("Device were not found");
            return NotFound("Device could not be found");
        }

        DeviceLog log = new DeviceLog(
            new Guid(),
            values.Humidity,
            values.Temperature,
            values.Current,
            values.Voltage,
            device,
            DateTime.Now);

        device.UpdateFromLastLog(log, DateTime.Now);
        device.ArrangeStatus(DateTime.Now);
        await _dbContext.SaveChangesAsync();
        await _notificationService.DeviceUpdated(device);
        return Ok();
    }
}