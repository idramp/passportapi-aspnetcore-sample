using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using AspNetDemo.Extensions;

namespace AspNetDemo
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            string bearerToken = Configuration.GetValue<string>("passportApiBearerToken");
            services.AddPassportAPI(options =>
            {
                options.BearerToken = bearerToken;
                options.ApiEndpointUrl = "https://passport-api-test.azurewebsites.net/";
            });
            services.AddSingleton<Services.ConnectionApiService>();
            services.AddSingleton<Services.ProofApiService>();
            services.AddSingleton<Services.CredentialApiService>();
            services.AddSingleton<Services.BasicMessageApiService>();

            services.AddAuthentication()
                // create a cookie to store the Mobile Identity Wallet's connection id
                .AddCookie(Models.AuthConstants.CookieScheme, options =>
                {
                    options.LoginPath = "/Passport/Connect";
                    options.LogoutPath = "/Logout";
                });
            services.AddAuthorization();

            services.AddRazorPages(options =>
            {
                // sets the page /Passport/Index as the default landing page.
                options.Conventions.AddPageRoute("/Passport/Index", "");
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });
        }
    }
}
