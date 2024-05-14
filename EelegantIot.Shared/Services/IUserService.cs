namespace EelegantIot.Shared.Services
{
    public interface IUserService
    {
        Task<bool> IsLoggedIn();
        Task<string?> LoginOrSignUpUser(string username, string password);
    }
}
