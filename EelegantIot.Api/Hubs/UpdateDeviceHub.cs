using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace EelegantIot.Api.Hubs;

[Authorize]
public class UpdateDeviceHub : Hub<IDeviceUpdateClient>;
