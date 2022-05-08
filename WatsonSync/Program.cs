using Dapper;
using Microsoft.AspNetCore.HttpLogging;
using WatsonSync.Components.Authentication;
using WatsonSync.Components.DataAccess;
using WatsonSync.Components.Mailing;
using WatsonSync.Components.Verification;

SqlMapper.AddTypeHandler(new DateTimeOffsetHandler());
SqlMapper.AddTypeHandler(new GuidHandler());
SqlMapper.AddTypeHandler(new TimeSpanHandler());

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddScoped<IContextFactory>(provider =>
        new SqliteContextFactory(
            provider.GetService<IConfiguration>().GetConnectionString("Default")));
builder.Services.AddScoped<IDatabase, SqliteDatabase>();
builder.Services.AddScoped<UserVerifier>();
builder.Services.AddTransient<IMailer, PostmarkMailer>();
builder.Services.AddHttpLogging(options =>
{
    options.LoggingFields = HttpLoggingFields.All;
    options.RequestBodyLogLimit = 4096;
    options.ResponseBodyLogLimit = 4096;
});
builder.Services.AddControllers();
builder.Services.AddRazorPages();

var application = builder.Build();

if (application.Environment.IsDevelopment())
{
    application.UseDeveloperExceptionPage();
    application.UseHttpLogging();
}

application.UseRouting();
application.UseMiddleware<TokenAuthenticationMiddleware>();
application.MapControllers();
application.MapRazorPages();
application.Run();