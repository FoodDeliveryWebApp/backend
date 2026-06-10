using Microsoft.Net.Http.Headers;

namespace Explorer.API.Startup;

public static class CorsConfiguration
{
    public static IServiceCollection ConfigureCors(this IServiceCollection services, string corsPolicy)
    {
        services.AddCors(options =>
        {
            options.AddPolicy(name: corsPolicy,
                builder =>
                {
                    builder.WithOrigins(ParseCorsOrigins())
                        .WithHeaders(HeaderNames.ContentType, HeaderNames.Authorization, "access_token")
                        .WithMethods("GET", "PUT", "POST", "PATCH", "DELETE", "OPTIONS");
                });
        });
        return services;
    }

    private static string[] ParseCorsOrigins()
    {
        var corsOriginsEnv = Environment.GetEnvironmentVariable("EXPLORER_CORS_ORIGINS");
        if (!string.IsNullOrEmpty(corsOriginsEnv))
        {
            return corsOriginsEnv.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        }

        return new[] { "http://localhost:4201", "http://localhost" };
    }
}