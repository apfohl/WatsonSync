using Dapper;
using JetBrains.Annotations;
using Microsoft.AspNetCore.HttpLogging;
using WatsonSync.Components.Authentication;
using WatsonSync.Components.DataAccess;

namespace WatsonSync;

public sealed class Startup
{
    public static void ConfigureServices(IServiceCollection services)
    {
        SqlMapper.AddTypeHandler(new DateTimeOffsetHandler());
        SqlMapper.AddTypeHandler(new GuidHandler());
        SqlMapper.AddTypeHandler(new TimeSpanHandler());

        services
            .AddScoped<IContextFactory>(provider =>
                new SqliteContextFactory(
                    provider.GetService<IConfiguration>().GetConnectionString("Default")))
            .AddScoped<IDatabase, SqliteDatabase>()
            .AddHttpLogging(options =>
            {
                options.LoggingFields = HttpLoggingFields.All;
                options.RequestBodyLogLimit = 4096;
                options.ResponseBodyLogLimit = 4096;
            })
            .AddControllers();
    }

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