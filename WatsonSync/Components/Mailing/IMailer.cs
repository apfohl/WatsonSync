using MonadicBits;

namespace WatsonSync.Components.Mailing;

public interface IMailer
{
    Task<Maybe<MailError>> Send(string to, string subject, string message);
}