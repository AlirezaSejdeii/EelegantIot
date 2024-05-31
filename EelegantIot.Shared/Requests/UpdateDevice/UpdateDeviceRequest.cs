namespace EelegantIot.Shared.Requests.UpdateDevice;

public class UpdateDeviceRequest
{
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
    public int[] DayOfWeeks { get; set; }
}