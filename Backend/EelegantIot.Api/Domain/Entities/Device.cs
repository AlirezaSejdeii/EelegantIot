using EelegantIot.Api.Domain.Enums;

namespace EelegantIot.Api.Domain.Entities;

public class Device : BaseEntity
{
    public Device(Guid id, string identifier, DateTime createdAt)
    {
        Id = id;
        Identifier = identifier;
        Humidity = 0;
        Temperature = 0;
        Current = 0;
        Voltage = 0;
        SettingMode = SettingMode.Manual;
        CreatedAt = createdAt;
    }

    protected Device()
    {
    }

    public SettingMode SettingMode { get; private set; }
    public string Identifier { get; private set; }
    public double Humidity { get; private set; }
    public double Temperature { get; private set; }
    public double Current { get; private set; }
    public double Voltage { get; private set; }
    public bool IsOn { get; private set; }

    public int[]? DayOfWeeks { get; private set; }
    public TimeOnly StartAt { get; private set; }
    public TimeOnly EndAt { get; private set; }
    public List<UserDevices> DeviceUsers { get; set; }
    public List<DeviceLog> Logs { get; set; }


    public void UpdateFromLastLog(DeviceLog log, DateTime now)
    {
        Logs.Add(log);
        Humidity = log.Humidity;
        Temperature = log.Temperature;
        Current = log.Current;
        Voltage = log.Voltage;
        UpdatedAt = now;
    }

    public TimeOnly? GetTodayStartAt(DateTime now)
    {
        DayOfWeek today = now.Date.DayOfWeek;
        if (DayOfWeeks != null && DayOfWeeks.Any(x => x == (int)today) && SettingMode is SettingMode.Timer)
        {
            return IsOn ? EndAt : StartAt;
        }

        return null;
    }

    public void ArrangeStatus(DateTime now)
    {
        if (
            SettingMode is SettingMode.Timer &&
            DayOfWeeks != null)
        {
            TimeSpan start = StartAt.ToTimeSpan();
            TimeSpan end = EndAt.ToTimeSpan();
            TimeSpan currentTime = now.TimeOfDay;

            if (
                (end < start && (currentTime >= start || currentTime <= end)) ||
                (currentTime >= start && currentTime <= end))
            {
                IsOn = true;
            }
            else
            {
                IsOn = false;
            }

            if (DayOfWeeks.All(x => (DayOfWeek)x != now.DayOfWeek))
            {
                IsOn = false;
            }
        }
    }

    public void UpdateTimer(int[] dayOfWeeks, TimeOnly startAt, TimeOnly endAt, DateTime now)
    {
        DayOfWeeks = dayOfWeeks;
        StartAt = startAt;
        EndAt = endAt;
        SettingMode = SettingMode.Timer;
        UpdatedAt = now;
    }

    public void ToggleIsOnManually(DateTime now)
    {
        IsOn = !IsOn;
        SettingMode = SettingMode.Manual;
        UpdatedAt = now;
    }

    public void ChangeToManual(DateTime now)
    {
        SettingMode = SettingMode.Manual;
    }
}