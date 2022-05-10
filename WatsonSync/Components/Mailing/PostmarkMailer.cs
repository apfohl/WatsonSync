using Microsoft.Extensions.Options;
using MonadicBits;
using PostmarkDotNet;
using WatsonSync.Models;

namespace WatsonSync.Components.Mailing;

public sealed class PostmarkMailer : IMailer
{
    private readonly MailSettings settings;

    public PostmarkMailer(IOptions<MailSettings> options) =>
        settings = options.Value;

    public async Task<Maybe<MailError>> Send(string to, string subject, string message) =>
        (await Message(to, subject, message).Send(settings.PostmarkApiKey))
        .Status
        .MapError();

    private PostmarkMessage Message(string to, string subject, string body) =>
        new()
        {
            To = to,
            From = settings.From,
            Subject = subject,
            TextBody = body,
            MessageStream = "outbound"
        };
}