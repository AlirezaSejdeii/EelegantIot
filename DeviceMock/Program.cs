using System.Net.Http.Json;
using EelegantIot.Api.Models;

HttpClient httpClient = new();

while (true)
{
    string path = "https://localhost:7245/device-manager/update-values/test2";
    Random r = new();
    UpdateDeviceValues deviceValues = new(r.Next(0, 50), r.Next(0, 50), r.Next(0, 50), r.Next(0, 50));
    var result = await httpClient.PutAsJsonAsync(path, deviceValues);
    Console.WriteLine($"result: {result.IsSuccessStatusCode}");
    await Task.Delay(5000);
}