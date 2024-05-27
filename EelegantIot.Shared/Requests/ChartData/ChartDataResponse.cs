namespace EelegantIot.Shared.Requests.ChartData;

public class ChartDataResponse
{
    public List<DateTime> Labels { get; set; } = new();
    public List<Series> Series { get; set; } = new();
}