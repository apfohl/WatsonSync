using MonadicBits;
using NLog;

namespace WatsonSync.Components.Mailing;

using static Functional;

public class LoggingMailer : IMailer
{
    private static readonly Logger Log = LogManager.GetCurrentClassLogger();

    public Task<Maybe<MailError>> Send(string to, string subject, string message)
    {
        Log.Info($"NEW EMAIL\nTo: {to}\nSubject: {subject}\nMesssage: {message}");
        return Task.FromResult<Maybe<MailError>>(Nothing);
    }
}