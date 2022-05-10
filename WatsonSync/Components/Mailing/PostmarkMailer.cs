using Microsoft.Extensions.Options;
using MonadicBits;
using PostmarkDotNet;
using WatsonSync.Models;
using static WatsonSync.Components.Extensions.FunctionalExtensions;

namespace WatsonSync.Components.Mailing;

public sealed class PostmarkMailer : IMailer
{
    private readonly MailSettings settings;

    public PostmarkMailer(IOptions<MailSettings> options) =>
        settings = options.Value;

    public async Task<Maybe<MailError>> Send(string to, string subject, string message)
    {
        var response = await new PostmarkClient(settings.PostmarkApiKey)
            .SendMessageAsync(CreateMessage(to, subject, message));

        return response.Status switch
        {
            PostmarkStatus.Success => NoThing<MailError>(),
            PostmarkStatus.Unknown => MailError.Unknown,
            PostmarkStatus.UserError => MailError.UserError,
            PostmarkStatus.ServerError => MailError.ServerError,
            _ => throw new ArgumentOutOfRangeException(nameof(response.Status), response.Status, null)
        };
    }

    private PostmarkMessage CreateMessage(string to, string subject, string body) =>
        new()
        {
            To = to,
            From = settings.From,
            Subject = subject,
            TextBody = body,
            MessageStream = "outbound"
        };
}