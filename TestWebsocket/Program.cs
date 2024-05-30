using System.Text.Json;
using EelegantIot.Shared.Hubs;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.SignalR.Client;

string token =
    "eyJhbGciOiJBMTI4S1ciLCJlbmMiOiJBMTI4Q0JDLUhTMjU2IiwidHlwIjoiSldUIiwiY3R5IjoiSldUIn0.1P0aYGoWQ4-8G6NoMl6W9MCfJI_b4A1vz1NGnm2TMO1vyLcF9RgWnQ.Jn0cKTJ1FByVxOfDymfYBQ.8kMeFhIv4i5590WlMccy4FZummkTck8TfBzNeEbuDmpAuLrnv4f51Hn4KSel0UTH0gVvmHPxXwfwN26k5MvzV5yrJ8LAkqqYxMdGRQwMFApsrZLU33nXnpnaMeopfE08BPIM9-IXtnISkO4RWZmvkET_kToFgZOWaSgimVpPMjxYn2qCO5P2qj1xlpaL07Nx3ZFvHPSaqbdwIAzOOPWMe8bT_M8wFnLiiS6tLeQaPBG4ghBGhJVWAMAJgs8smL3yTz_-79z66TtO9_1BQPRCuY4HE0mMwjjkDMqmBinAtMBXYQ0pI1eFeq0OV2OIWjoAv00vq2p_of0EESuUGAuwXQb6gnLrpt0RjB7GHIvLu1Yct9elx5-0spjqDvS4un1FShecDE9OTE5_AFGSm5Mgzg.f3DjEkZxwM6ybopGF2Rgtg";
string url = "wss://localhost:7245/hub/update-device-notification";

HubConnectionBuilder hubConnectionBuilder = new();

HubConnection connection = hubConnectionBuilder.WithUrl(url, options =>
    {
        options.SkipNegotiation = true;
        options.Transports = HttpTransportType.WebSockets;
        options.AccessTokenProvider = () => Task.FromResult(token)!;
    })
    .Build();
hubConnectionBuilder.WithAutomaticReconnect();

await connection.StartAsync().ContinueWith(task =>
{
    if (task.IsFaulted)
    {
        Console.WriteLine("There was an error opening the connection:{0}", task.Exception.GetBaseException());
    }
    else
    {
        Console.WriteLine("Connected");
    }
});

connection.On<DeviceHubUpdateDto>("OnDeviceUpdated", DoSomething);

Console.WriteLine(connection.ConnectionId);

Console.Read();

void DoSomething(DeviceHubUpdateDto param)
{
    Console.WriteLine($"Received at: {DateTime.Now}");
    string json = JsonSerializer.Serialize(param, new JsonSerializerOptions { WriteIndented = true });
    Console.WriteLine(json);
}