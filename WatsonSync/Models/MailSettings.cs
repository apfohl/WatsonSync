namespace WatsonSync.Models;

public sealed record MailSettings
{
    public string PostmarkApiKey { get; set; }
    public string From { get; set; }
    public string ApplicationUrl { get; set; }

    public MailSettings(string postmarkApiKey, string from, string applicationUrl)
    {
        PostmarkApiKey = postmarkApiKey;
        From = from;
        ApplicationUrl = applicationUrl;
    }

    public MailSettings()
    {
    }
}