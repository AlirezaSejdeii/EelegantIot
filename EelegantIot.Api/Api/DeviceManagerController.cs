using EelegantIot.Api.Domain.Entities;
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

    public DeviceManagerController(AppDbContext dbContext, ILogger<DeviceManagerController> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    [HttpGet("rely-status/{id:guid}")]
    public async Task<IActionResult> GetRelyStatus(Guid id)
    {
        _logger.LogInformation("Fetching device");
        Device? device = await _dbContext.Devices.FirstOrDefaultAsync(x => x.Id == id);
        if (device is null)
        {
            _logger.LogInformation("Device were not found");
            return NotFound("Device could not be found");
        }

        return Ok(new GetRelyStatus(device.IsOn));
    }

    [HttpPut("update-values/{id:guid}")]
    public async Task<IActionResult> UpdateValues(Guid id, UpdateDeviceValues values)
    {
        _logger.LogInformation("Fetching device");
        Device? device = await _dbContext.Devices.FirstOrDefaultAsync(x => x.Id == id);
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
        await _dbContext.SaveChangesAsync();
        return Ok();
    }
}