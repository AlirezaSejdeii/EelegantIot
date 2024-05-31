using System.Net.Http.Headers;
using System.Net.Http.Json;
using EelegantIot.Shared.Requests.ChartData;
using EelegantIot.Shared.Requests.DeviceDetails;
using EelegantIot.Shared.Requests.DeviceList;
using EelegantIot.Shared.Requests.NewDevice;
using EelegantIot.Shared.Requests.UpdateDevice;
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
        HttpResponseMessage responseMessage = await httpClient.PostAsJsonAsync(Config.Api + "/devices/new", request);

        if (!responseMessage.IsSuccessStatusCode)
        {
            return new ResponseData<NoContent>(new ErrorModel("مشکلی در ارتباط با سرور وجود دارد"));
        }

        ResponseData<NoContent> result =
            await responseMessage.Content.ReadFromJsonAsync<ResponseData<NoContent>>();

        return result;
    }

    public async Task<ResponseData<DeviceDetailsDto>> GetDeviceById(Guid id)
    {
        string token = await _userService.GetLoginToken();
        HttpClient httpClient = new()
        {
            DefaultRequestHeaders = { Authorization = new AuthenticationHeaderValue("Bearer", token) }
        };
        HttpResponseMessage responseMessage = await httpClient.GetAsync(Config.Api + $"/devices/details/{id}");

        if (!responseMessage.IsSuccessStatusCode)
        {
            return new ResponseData<DeviceDetailsDto>(new ErrorModel("مشکلی در ارتباط با سرور وجود دارد"));
        }

        ResponseData<DeviceDetailsDto> result =
            await responseMessage.Content.ReadFromJsonAsync<ResponseData<DeviceDetailsDto>>();

        return result;
    }

    public async Task<ResponseData<ChartDataResponse>> GetDeviceChart(Guid id, ChartDataRequest request)
    {
        string token = await _userService.GetLoginToken();
        HttpClient httpClient = new()
        {
            DefaultRequestHeaders = { Authorization = new AuthenticationHeaderValue("Bearer", token) }
        };
        HttpResponseMessage responseMessage =
            await httpClient.GetAsync(Config.Api +
                                      $"/devices/chart/{id}?StartDate={request.StartDate}&EndDate={request.EndDate}");

        if (!responseMessage.IsSuccessStatusCode)
        {
            return new ResponseData<ChartDataResponse>(new ErrorModel("مشکلی در ارتباط با سرور وجود دارد"));
        }

        ResponseData<ChartDataResponse> result =
            await responseMessage.Content.ReadFromJsonAsync<ResponseData<ChartDataResponse>>();

        return result;
    }

    public async Task<ResponseData<NoContent>> DeleteDeviceAsync(Guid id)
    {
        string token = await _userService.GetLoginToken();
        HttpClient httpClient = new()
        {
            DefaultRequestHeaders = { Authorization = new AuthenticationHeaderValue("Bearer", token) }
        };
        HttpResponseMessage responseMessage =
            await httpClient.DeleteAsync(Config.Api +
                                         $"/devices/remove/{id}");

        if (!responseMessage.IsSuccessStatusCode)
        {
            return new ResponseData<NoContent>(new ErrorModel("مشکلی در ارتباط با سرور وجود دارد"));
        }

        ResponseData<NoContent> result =
            await responseMessage.Content.ReadFromJsonAsync<ResponseData<NoContent>>();

        return result;
    }

    public async Task<ResponseData<NoContent>> ToggleManuallyAsync(Guid id)
    {
        string token = await _userService.GetLoginToken();
        HttpClient httpClient = new()
        {
            DefaultRequestHeaders = { Authorization = new AuthenticationHeaderValue("Bearer", token) }
        };
        HttpResponseMessage responseMessage =
            await httpClient.PutAsync(Config.Api +
                                      $"/devices/toggle-is-on/{id}", null);

        if (!responseMessage.IsSuccessStatusCode)
        {
            return new ResponseData<NoContent>(new ErrorModel("مشکلی در ارتباط با سرور وجود دارد"));
        }

        ResponseData<NoContent> result =
            await responseMessage.Content.ReadFromJsonAsync<ResponseData<NoContent>>();

        return result;
    }

    public async Task<ResponseData<NoContent>> UpdateTimer(Guid id, UpdateDeviceRequest timer)
    {
        string token = await _userService.GetLoginToken();
        HttpClient httpClient = new()
        {
            DefaultRequestHeaders = { Authorization = new AuthenticationHeaderValue("Bearer", token) }
        };
        HttpResponseMessage responseMessage =
            await httpClient.PutAsJsonAsync(Config.Api +
                                            $"/devices/update-timer/{id}", timer);

        if (!responseMessage.IsSuccessStatusCode)
        {
            return new ResponseData<NoContent>(new ErrorModel("مشکلی در ارتباط با سرور وجود دارد"));
        }

        ResponseData<NoContent> result =
            await responseMessage.Content.ReadFromJsonAsync<ResponseData<NoContent>>();

        return result;
    }
}