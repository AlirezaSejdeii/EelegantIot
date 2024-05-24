using EelegantIot.Shared.Requests.DeviceList;
using EelegantIot.Shared.Response;

namespace EelegantIot.Shared.Services;

public interface IDeviceService
{
    Task<ResponseData<List<DeviceItemDto>>> GetDeviceList();
}