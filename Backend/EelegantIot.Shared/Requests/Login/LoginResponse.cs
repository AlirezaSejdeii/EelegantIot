namespace EelegantIot.Shared.Requests.Login;

public class LoginResponse
{
    public string Token { get; set; }
    public DateTime ExpireDate { get; set; }
}