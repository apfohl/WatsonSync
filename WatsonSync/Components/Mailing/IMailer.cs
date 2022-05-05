namespace WatsonSync.Components.Mailing;

public interface IMailer
{
    Task Send(string subject, string message);
}