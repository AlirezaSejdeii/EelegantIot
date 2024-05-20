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
            return NotFound("Device Id is null");
        }

        return Ok(new GetRelyStatus(device.IsOn));
    }
}