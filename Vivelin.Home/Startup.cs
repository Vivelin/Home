using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.UserSecrets;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Vivelin.Home.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.HttpOverrides;
using Vivelin.AspNetCore.Headers;

namespace Vivelin.Home
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            if (env.IsDevelopment())
            {
                builder.AddUserSecrets<Startup>();
            }

            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            })
            .AddCookie(options =>
            {
                options.LoginPath = "/login";
                options.LogoutPath = "/logout";
                options.ExpireTimeSpan = TimeSpan.FromHours(1);
            })
            .AddTwitch(options =>
            {
                options.ClientId = Configuration["TwitchClientId"];
                options.ClientSecret = Configuration["TwitchClientSecret"];
                options.SaveTokens = true;
            });

            services.AddMvc();
            services.AddDbContext<HomeContext>(options =>
            {
                options.UseSqlite(Configuration.GetConnectionString("HomeContext"));
            });

            services.AddSingleton<IConfiguration>(Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            ConfigureDbContextMigrationsAsync(app)
                .ConfigureAwait(false)
                .GetAwaiter()
                .GetResult();

            app.UseStaticFiles();

            app.UseForwardedHeaders(new ForwardedHeadersOptions { ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto });
            app.UseResponseHeaders(options =>
            {
                options
                    .Add("X-XSS-Protection", "1; mode=block")
                    .Add("Referrer-Policy", "strict-origin")
                    .Add("Content-Security-Policy", "default-src 'self'; style-src 'self' https://fonts.googleapis.com; font-src 'self' https://fonts.gstatic.com; connect-src 'self' https://api.twitch.tv; img-src 'self' https://static-cdn.jtvnw.net");
            });

            app.Use((context, next) =>
            {
                context.Request.Scheme = "https";
                return next();
            });

            app.UseAuthentication();
            app.UseMvc(ConfigureRoutes);
        }

        private static void ConfigureRoutes(Microsoft.AspNetCore.Routing.IRouteBuilder routes)
        {
            routes.MapRoute(
                name: "default",
                template: "{controller=Home}/{action=Index}/{id?}");
        }

        private async Task ConfigureDbContextMigrationsAsync(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            using (var context = serviceScope.ServiceProvider.GetService<HomeContext>())
            {
                await context.Database.MigrateAsync();
                await context.SeedAsync();
            }
        }
    }
}
