namespace EelegantIot.Api.Domain.Entities;

public class User : BaseEntity
{
    public User(Guid id,string username, string password)
    {
        Id = id;
        Username = username;
        Password = password;
    }

    protected User()
    {
    }

    public string Username { get; private set; }
    public string Password { get; private set; }
    public List<Device> Devices { get; set; }
}