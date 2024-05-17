namespace EelegantIot.Api.Domain.Entities;

public class UserDevices
{
    public Guid UserId { get; set; }
    public User User { get; set; }
    public Guid DeviceId { get; set; }
    public Device Device { get; set; }
    public string Name { get; set; }
}