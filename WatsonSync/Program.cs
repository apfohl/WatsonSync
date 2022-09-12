using Dapper;
using NLog;
using NLog.Web;
using WatsonSync.Components.Authentication;
using WatsonSync.Components.DataAccess;
using WatsonSync.Components.Mailing;
using WatsonSync.Components.Verification;
using WatsonSync.Models;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;

var log = LogManager
    .Setup()
    .LoadConfigurationFromAppSettings()
    .GetCurrentClassLogger();

log.Info("init main");

SqlMapper.AddTypeHandler(new DateTimeOffsetHandler());
SqlMapper.AddTypeHandler(new GuidHandler());
SqlMapper.AddTypeHandler(new TimeSpanHandler());
SqlMapper.AddTypeHandler(new UserSettingsHandler());
SqlMapper.AddTypeHandler(new UserSettingTypesHandler());

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Logging.ClearProviders();
    builder.Logging.SetMinimumLevel(LogLevel.Trace);
    builder.Host.UseNLog();

    builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));
    builder.Services.Configure<Settings>(builder.Configuration.GetSection("Settings"));
    builder.Services.AddScoped<IContextFactory>(provider =>
        new SqliteContextFactory(
            provider.GetService<IConfiguration>().GetConnectionString("Default")));
    builder.Services.AddScoped<IDatabase, SqliteDatabase>();
    builder.Services.AddScoped<UserVerifier>();

    if (builder.Environment.IsDevelopment())
    {
        builder.Services.AddTransient<IMailer, LoggingMailer>();
    }
    else
    {
        builder.Services.AddTransient<IMailer, PostmarkMailer>();
    }

    builder.Services.AddControllers();
    builder.Services.AddRazorPages();

    var application = builder.Build();

    if (application.Environment.IsDevelopment())
    {
        application.UseDeveloperExceptionPage();
    }

    application.UseRouting();
    application.UseMiddleware<TokenAuthenticationMiddleware>();
    application.MapControllers();
    application.MapRazorPages();
    application.Run();
}
catch (Exception exception)
{
    log.Error(exception, "Stopped program because of exception");
    throw;
}
finally
{
    LogManager.Shutdown();
}