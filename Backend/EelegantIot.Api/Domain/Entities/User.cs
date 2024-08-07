namespace EelegantIot.Api.Domain.Entities;

public class User : BaseEntity
{
    public User(Guid id,string username, string password,DateTime createdAt)
    {
        Id = id;
        Username = username;
        Password = password;
        CreatedAt = createdAt;
    }

    protected User()
    {
    }

    public string Username { get; private set; }
    public string Password { get; private set; }
    public List<UserDevices> UserDevices { get; set; }
}