using MonadicBits;
using PostmarkDotNet;

namespace WatsonSync.Components.Mailing;

using static Functional;

public static class MailerExtensions
{
    public static Maybe<MailError> MapError(this PostmarkStatus status) =>
        status switch
        {
            PostmarkStatus.Success => Nothing,
            PostmarkStatus.Unknown => MailError.Unknown,
            PostmarkStatus.UserError => MailError.UserError,
            PostmarkStatus.ServerError => MailError.ServerError,
            _ => throw new ArgumentOutOfRangeException(nameof(status), status, null)
        };

    public static Task<PostmarkResponse> Send(this PostmarkMessage message, string apiKey) =>
        new PostmarkClient(apiKey).SendMessageAsync(message);
}