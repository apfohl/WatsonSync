using NLog;

namespace WatsonSync.Components.Mailing;

public class LoggingMailer : IMailer
{
    private static readonly Logger Log = LogManager.GetCurrentClassLogger();

    public Task Send(string subject, string message)
    { 
        Log.Info($"NEW EMAIL\nSubject: {subject}\nMesssage: {message}");
        return Task.CompletedTask;
    }
}