using System.Net.Http.Headers;
using System.Net.Http.Json;
using EelegantIot.Shared.Requests.DeviceList;
using EelegantIot.Shared.Requests.NewDevice;
using EelegantIot.Shared.Response;

namespace EelegantIot.Shared.Services;

public class DeviceService : IDeviceService
{
    private readonly UserService _userService;

    public DeviceService(UserService userService)
    {
        _userService = userService;
    }

    public async Task<ResponseData<List<DeviceItemDto>>> GetDeviceList()
    {
        string token = await _userService.GetLoginToken();
        HttpClient httpClient = new()
        {
            DefaultRequestHeaders = { Authorization = new AuthenticationHeaderValue("Bearer", token) }
        };
        HttpResponseMessage responseMessage = await httpClient.GetAsync(Config.Api + "/devices/list");

        if (!responseMessage.IsSuccessStatusCode)
        {
            return new ResponseData<List<DeviceItemDto>>(new ErrorModel("مشکلی در ارتباط با سرور وجود دارد"));
        }

        ResponseData<List<DeviceItemDto>> result =
            await responseMessage.Content.ReadFromJsonAsync<ResponseData<List<DeviceItemDto>>>();

        return result;
    }
    
    public async Task<ResponseData<NoContent>> AddNewDevice(NewDeviceRequest request)
    {
        string token = await _userService.GetLoginToken();
        HttpClient httpClient = new()
        {
            DefaultRequestHeaders = { Authorization = new AuthenticationHeaderValue("Bearer", token) }
        };
        HttpResponseMessage responseMessage = await httpClient.PostAsJsonAsync(Config.Api + "/devices/new",request);

        if (!responseMessage.IsSuccessStatusCode)
        {
            return new ResponseData<NoContent>(new ErrorModel("مشکلی در ارتباط با سرور وجود دارد"));
        }

        ResponseData<NoContent> result =
            await responseMessage.Content.ReadFromJsonAsync<ResponseData<NoContent>>();

        return result;
    }
}