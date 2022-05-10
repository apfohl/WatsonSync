namespace WatsonSync.Models;

public record MailSettings
{
    public string PostmarkApiKey { get; set; }

    public string From { get; set; }

    public MailSettings(string postmarkApiKey, string from)
    {
        PostmarkApiKey = postmarkApiKey;
        From = from;
    }

    public MailSettings()
    {
    }
}