// using System.Text;
// using Microsoft.Extensions.Primitives;
// using Microsoft.IdentityModel.Tokens;
//
// namespace EelegantIot.Api.Extension;
//
// public static class AuthenticationExtension
// {
//     public static IServiceCollection AddCustomAuthentication(this IServiceCollection services, string secret,
//         string encryption)
//     {
//         byte[] secretKey = Encoding.ASCII.GetBytes(secret);
//         byte[] encryptionKey = Encoding.ASCII.GetBytes(encryption);
//         var tokenValidationParameters = new TokenValidationParameters
//         {
//             ValidateIssuerSigningKey = true,
//             IssuerSigningKey = new SymmetricSecurityKey(secretKey),
//             ValidateIssuer = false,
//             ValidateAudience = false,
//             RequireExpirationTime = true,
//             ValidateLifetime = true,
//             ClockSkew = TimeSpan.Zero,
//             TokenDecryptionKey = new SymmetricSecurityKey(encryptionKey)
//         };
//         services.AddAuthentication(options =>
//         {
//             options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
//             options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
//             options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
//         }).AddJwtBearer(options =>
//         {
//             options.RequireHttpsMetadata = false;
//             options.SaveToken = true;
//             options.TokenValidationParameters = tokenValidationParameters;
//
//             options.Events = new JwtBearerEvents
//             {
//                 OnMessageReceived = OnMessageReceived()
//             };
//         });
//         return services;
//     }
//
//     private static Func<MessageReceivedContext, Task> OnMessageReceived()
//     {
//         return context =>
//         {
//             PathString path = context.HttpContext.Request.Path;
//
//             if (path.StartsWithSegments("/hub/update-device-notification"))
//             {
//                 StringValues accessToken = context.Request.Query["access_token"];
//
//                 if (!string.IsNullOrWhiteSpace(accessToken))
//                 {
//                     context.Token = accessToken;
//                 }
//             }
//
//             return Task.CompletedTask;
//         };
//     }
// }