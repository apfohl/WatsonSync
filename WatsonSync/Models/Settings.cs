namespace WatsonSync.Models;

public sealed record Settings
{
    public string HolidayIdentifier { get; set; }

    public Settings(string holidayIdentifier)
    {
        HolidayIdentifier = holidayIdentifier;
    }

    public Settings()
    {
    }
}