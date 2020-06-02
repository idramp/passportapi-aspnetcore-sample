using AspNetDemo.Models;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

            services.AddSingleton<PassportApi.swaggerClient>((IServiceProvider services) =>
            {
                System.Net.Http.HttpClient client = new System.Net.Http.HttpClient();
                client.DefaultRequestHeaders.Authorization = 
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer",
                        config.BearerToken
                    );
                return new PassportApi.swaggerClient(config.ApiEndpointUrl, client);
            });
        }
    }
}
