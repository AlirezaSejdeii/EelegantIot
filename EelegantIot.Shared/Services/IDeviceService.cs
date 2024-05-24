using EelegantIot.Shared.Requests.DeviceDetails;
using EelegantIot.Shared.Requests.DeviceList;
using EelegantIot.Shared.Requests.NewDevice;
using EelegantIot.Shared.Response;

namespace EelegantIot.Shared.Services;

public interface IDeviceService
{
    Task<ResponseData<List<DeviceItemDto>>> GetDeviceList();
    Task<ResponseData<DeviceDetailsDto>> GetDeviceById(Guid id);
    Task<ResponseData<NoContent>> AddNewDevice(NewDeviceRequest request);
}