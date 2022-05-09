using System.Net.Mail;
using System.Text.RegularExpressions;
using MonadicBits;
using WatsonSync.Components.Extensions;

namespace WatsonSync.Components.Validation;

using static Functional;

public static class PropertyValidation
{
    public static Maybe<string> ValidateEmailAddress(string emailAddress) =>
        MailAddress.TryCreate(emailAddress, out var result) ? result.Address : Nothing;

    public static Maybe<string> ValidateToken(string token) =>
        new Regex("^[A-F0-9]{32}$")
            .MatchInput(token)
            .Map(match => match.Value);
}