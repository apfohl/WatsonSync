using JetBrains.Annotations;
using Microsoft.AspNetCore.HttpLogging;

namespace WatsonSync;

public sealed class Startup
{
    public static void ConfigureServices(IServiceCollection services) =>
        services
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
        app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
    }
}