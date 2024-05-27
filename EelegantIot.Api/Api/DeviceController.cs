using EelegantIot.Api.Domain.Entities;
using EelegantIot.Api.Infrastructure;
using EelegantIot.Shared.Requests.ChartData;
using EelegantIot.Shared.Requests.DeviceDetails;
using EelegantIot.Shared.Requests.DeviceList;
using EelegantIot.Shared.Requests.NewDevice;
using EelegantIot.Shared.Response;
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
                return Ok(new ResponseData<NoContent>(new ErrorModel("کاربر قبلا دستگاه را ثبت کرده است")));
            }

            device.DeviceUsers.Add(new UserDevices(user, device, newDeviceRequest.Name));
            await dbContext.SaveChangesAsync();
            return Ok(new ResponseData<NoContent>(new NoContent()));
        }

        device = new(Guid.NewGuid(), newDeviceRequest.Pin, DateTime.Now);
        device.DeviceUsers = new();
        device.DeviceUsers.Add(new UserDevices(user, device, newDeviceRequest.Name));
        dbContext.Devices.Add(device);
        await dbContext.SaveChangesAsync();
        return Ok(new ResponseData<NoContent>(new NoContent()));
    }

    [HttpGet("list")]
    public async Task<ActionResult<ResponseData<List<DeviceItemDto>>>> GetDeviceList()
    {
        Guid userId = Guid.Parse(User.Identity!.Name!);
        IQueryable<Device> devices =
            dbContext.Devices.Where(x => x.DeviceUsers.Any(x => x.UserId == userId)).AsQueryable();

        List<DeviceItemDto> deviceList = await devices.Select(x => new DeviceItemDto(
                x.Id,
                x.IsOn,
                x.DeviceUsers.First(deviceUser => deviceUser.UserId == userId).Name,
                x.Identifier,
                x.GetTodayStartAt(DateTime.Now)))
            .ToListAsync();
        return new ResponseData<List<DeviceItemDto>>(deviceList);
    }

    [HttpGet("details/{id:guid}")]
    public async Task<ActionResult<DeviceDetailsDto>> GetDeviceDetails(Guid id)
    {
        Guid userId = Guid.Parse(User.Identity!.Name!);
        DeviceDetailsDto? device = await dbContext.Devices
            .Where(x => x.Id == id && x.DeviceUsers.Any(userDevices => userDevices.UserId == userId))
            .Select(x => new DeviceDetailsDto(
                x.Current,
                x.Voltage,
                x.Temperature,
                x.Humidity,
                (SettingMode)x.SettingMode,
                x.StartAt,
                x.EndAt,
                x.DayOfWeeks))
            .FirstOrDefaultAsync();
        if (device is null)
        {
            return Ok(new ResponseData<DeviceDetailsDto>(new ErrorModel("دستگاه یافت نشد")));
        }

        return Ok(new ResponseData<DeviceDetailsDto>(device));
    }

    [HttpDelete("remove/{id:guid}")]
    public async Task<IActionResult> UnboundUserFromDevice(Guid id)
    {
        Guid userId = Guid.Parse(User.Identity!.Name!);
        Device? device = await dbContext.Devices.Include(device => device.DeviceUsers).FirstOrDefaultAsync(x =>
            x.Id == id && x.DeviceUsers.Any(userDevices => userDevices.UserId == userId));
        if (device is null)
        {
            return Ok(new ResponseData<NoContent>(new ErrorModel("دستگاه یافت نشد")));
        }

        device.DeviceUsers.Remove(device.DeviceUsers.First(x => x.UserId == userId));
        return Ok(new ResponseData<NoContent>(new NoContent()));
    }

    [HttpGet]
    public async Task<ActionResult<ResponseData<ChartDataResponse>>> GetChartData(
        Guid id,
        [FromQuery] ChartDataRequest request)
    {
        Device? device = await dbContext.Devices.Include(x => x.Logs
                .Where(logs => logs.CreatedAt >= request.StartDate && logs.CreatedAt <= request.EndDate))
            .FirstOrDefaultAsync(x => x.Id == id);

        if (device is null)
        {
            return Ok(new ResponseData<ChartDataResponse>(new ErrorModel("دستگاه یافت نشد")));
        }

        ChartDataResponse response = new();
        response.Labels.AddRange(device.Logs.Select(x => x.CreatedAt).ToList().DistinctBy(x=>x.Date));

        Series temperature = new Series
        {
            Name = "Temperature",
            Numbers = device.Logs.Select(x => x.Temperature).ToList()
        };

        Series humidity = new Series
        {
            Name = "Humidity",
            Numbers = device.Logs.Select(x => x.Humidity).ToList()
        };

        Series current = new Series
        {
            Name = "Current",
            Numbers = device.Logs.Select(x => x.Current).ToList()
        };

        Series voltage = new Series
        {
            Name = "Voltage",
            Numbers = device.Logs.Select(x => x.Voltage).ToList()
        };

        response.Series.AddRange([temperature, humidity, current, voltage]);
        return new ResponseData<ChartDataResponse>(response);
    }
}