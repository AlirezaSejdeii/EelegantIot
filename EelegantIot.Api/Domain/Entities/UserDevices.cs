namespace EelegantIot.Api.Domain.Entities;

public class UserDevices
{
    public UserDevices(User user, Device device, string name)
    {
        User = user;
        Device = device;
        Name = name;
    }

    protected UserDevices()
    {
    }

    public Guid UserId { get; set; }
    public User User { get; set; }
    public Guid DeviceId { get; set; }
    public Device Device { get; set; }
    public string Name { get; set; }
}