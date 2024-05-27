using EelegantIot.Api.Extension;
using EelegantIot.Api.Hubs;
using EelegantIot.Api.Infrastructure;
using EelegantIot.Api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Phoenix API", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = @"JWT Authorization header using the Bearer scheme. \r\n\r\n 
                      Enter 'Bearer' [space] and then your token in the text input below.
                      \r\n\r\nExample: 'Bearer 12345abcdef'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header,
            },
            new List<string>()
        }
    });
});
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration["Database:ConnectionString"]!);
});

builder.Services.Configure<JwtConfigDto>(builder.Configuration.GetSection("JwtConfig"));
builder.Services.AddCustomAuthentication(
    builder.Configuration["JwtConfig:Secret"]!,
    builder.Configuration["JwtConfig:EncryptionKey"]!);
builder.Services.AddScoped<IDeviceUpdateNotificationService, DeviceUpdateNotificationService>();
builder.Services.AddSignalR();
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseHttpsRedirection();
app.MapControllers();

var scope = app.Services.CreateScope();
AppDbContext appDbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
appDbContext.Database.Migrate();
appDbContext.Database.EnsureCreated();
app.MapHub<UpdateDeviceHub>("/hub/update-device-notification");
app.Run();