using Blazored.LocalStorage;

namespace EelegantIot.Shared.Services
{
    public class UserService : IUserService
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
            return token != null;
        }

        public async Task<string?> LoginOrSignUpUser(string username, string password)
        {
            await Task.Delay(1000);
            if (username == password)
            {
                return "نام کاربری و کلمه عبور یکسان است";
            }
            return null;
        }
    }
}
