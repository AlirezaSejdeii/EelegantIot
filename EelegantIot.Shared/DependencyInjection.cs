using Blazored.LocalStorage;
using EelegantIot.Shared.Services;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Components.Authorization;

namespace EelegantIot.Shared
{
    public static class DependencyInjection
    {
        public static void AddDependencies(this IServiceCollection services)
        {
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            services.AddBlazoredLocalStorageAsSingleton(config =>
            {
                config.JsonSerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
                config.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
                config.JsonSerializerOptions.IgnoreReadOnlyProperties = true;
                config.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
                config.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                config.JsonSerializerOptions.ReadCommentHandling = JsonCommentHandling.Skip;
                config.JsonSerializerOptions.WriteIndented = false;
            });

            // services
            services.AddScoped<UserService>();
            services.AddTransient<DeviceService>();
            services.AddScoped<AuthenticationStateProvider, UserService>(
                provider => provider.GetRequiredService<UserService>()
            );
            services.AddAuthorizationCore();

        }
    }
}
