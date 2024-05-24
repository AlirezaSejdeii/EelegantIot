using System.Globalization;
using System.Net.Http.Json;
using System.Security.Claims;
using Blazored.LocalStorage;
using EelegantIot.Shared.Requests.Login;
using EelegantIot.Shared.Response;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace EelegantIot.Shared.Services;

public class UserService : AuthenticationStateProvider
{
    private const string LoginTokenKeyToken = "LOGIN_TOKEN_KEY_TOKEN";
    private const string LoginTokenKeyExpireDate = "LOGIN_TOKEN_KEY_EXPIRE_DATE";

    readonly ILocalStorageService _localStorageService;

    public UserService(ILocalStorageService localStorageService)
    {
        _localStorageService = localStorageService;
    }

    public async Task<bool> IsLoggedIn()
    {
        string? token = await _localStorageService.GetItemAsStringAsync(LoginTokenKeyToken);
        string? dateTime = await _localStorageService.GetItemAsStringAsync(LoginTokenKeyExpireDate);

        if (token != null && dateTime != null && DateTime.TryParse(dateTime, out DateTime expireDate))
        {
            return expireDate > DateTime.Today;
        }

        if (token != null && dateTime != null)
        {
            await _localStorageService.RemoveItemAsync(LoginTokenKeyToken);
            await _localStorageService.RemoveItemAsync(LoginTokenKeyExpireDate);
        }

        return false;
    }

    public async Task<string?> LoginOrSignUpUser(string username, string password)
    {
        HttpClient httpClient = new();
        HttpResponseMessage responseMessage = await httpClient.PostAsJsonAsync(Config.Api + "/users/login",
            new LoginRequest { Username = username, Password = password });

        if (!responseMessage.IsSuccessStatusCode)
        {
            return "مشکلی در ارتباط با وب سرور وجود دارد";
        }

        ResponseData<LoginResponse>? result =
            await responseMessage.Content.ReadFromJsonAsync<ResponseData<LoginResponse>>();

        if (!result.Value.Success)
        {
            return result.Value.Error!.ErrorMessage;
        }

        await _localStorageService.SetItemAsStringAsync(LoginTokenKeyToken, result.Value.Data!.Token);
        await _localStorageService.SetItemAsStringAsync(LoginTokenKeyExpireDate,
            result.Value.Data!.ExpireDate.ToString(CultureInfo.InvariantCulture));
        await GetAuthenticationStateAsync();
        return null;
    }

    public async Task LogOut()
    {
        await _localStorageService.RemoveItemAsync(LoginTokenKeyToken);
        await _localStorageService.RemoveItemAsync(LoginTokenKeyExpireDate);
        await GetAuthenticationStateAsync();
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        if (await IsLoggedIn())
        {
            List<Claim> claims = new List<Claim>();

            claims.Add(new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString()));

            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "jwt");


            ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            AuthenticationState appState = new AuthenticationState(claimsPrincipal);
            NotifyAuthenticationStateChanged(Task.FromResult(appState));
            return appState;
        }
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()))));

        return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
    }

    public async Task<string> GetLoginToken()
    {
        string? token = await _localStorageService.GetItemAsStringAsync(LoginTokenKeyToken);
        return token!;
    }
}