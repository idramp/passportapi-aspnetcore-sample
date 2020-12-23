using System;
using System.Net.Http;
using AspNetDemo.Models;
using IdRamp.Passport;
using Microsoft.Extensions.DependencyInjection;


namespace AspNetDemo.Extensions
{
    public static class PassportExtensions
    {
        public static void AddPassportAPI(this IServiceCollection services, Action<ApiConfig> options)
        {
            ApiConfig config = new ApiConfig();
            config.ApiEndpointUrl = "https://passport-api.idramp.com/";

            options?.Invoke(config);

            if (string.IsNullOrEmpty(config.BearerToken))
                throw new ArgumentNullException(nameof(ApiConfig.BearerToken), "The PassportAPI bearer token is required.");

            services.AddSingleton<PassportApiClient>((IServiceProvider services) =>
            {
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer",
                        config.BearerToken
                    );
                return new PassportApiClient(config.ApiEndpointUrl, client);
            });
        }
    }
}
