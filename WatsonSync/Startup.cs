using JetBrains.Annotations;
using Microsoft.AspNetCore.HttpLogging;
using WatsonSync.Components;
using WatsonSync.Middlewares;

namespace WatsonSync;

public sealed class Startup
{
    public static void ConfigureServices(IServiceCollection services) =>
        services
            .AddScoped<UserAuthenticator>()
            .AddScoped<IUserRepository, SqliteUserRepository>()
            .AddScoped<IFrameRepository, SqliteFrameRepository>()
            .AddHttpLogging(options =>
            {
                options.LoggingFields = HttpLoggingFields.All;
                options.RequestBodyLogLimit = 4096;
                options.ResponseBodyLogLimit = 4096;
            })
            .AddControllers();
    
    [UsedImplicitly]
    public static void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseHttpLogging();
        }

        loggerFactory.AddLog4Net();
        
        app.UseRouting();
        app.UseMiddleware<TokenAuthenticationMiddleware>();
        app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
    }
}