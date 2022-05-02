using Dapper;
using JetBrains.Annotations;
using Microsoft.AspNetCore.HttpLogging;
using WatsonSync.Components;
using WatsonSync.Middlewares;

namespace WatsonSync;

public sealed class Startup
{
    private static readonly string DatabasePath =
        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "watson-sync.sqlite");

    public static void ConfigureServices(IServiceCollection services)
    {
        SqlMapper.AddTypeHandler(new DateTimeOffsetHandler());
        SqlMapper.AddTypeHandler(new GuidHandler());
        SqlMapper.AddTypeHandler(new TimeSpanHandler());

        services
            .AddScoped<IContextFactory>(_ => new SqliteContextFactory($"Data Source={DatabasePath}; Pooling=false"))
            .AddScoped<UserAuthenticator>()
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