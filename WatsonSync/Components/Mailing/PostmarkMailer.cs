namespace WatsonSync.Components.Mailing;

public sealed class PostmarkMailer : IMailer
{
    public Task Send(string subject, string message) =>
        Task.CompletedTask;
}