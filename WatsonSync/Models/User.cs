namespace WatsonSync.Models;

public sealed record User
{
    public int Id { get; set; }
    public string Email { get; set; }
    public string Token { get; set; }
    public string VerificationToken { get; set; }
    public bool IsVerified { get; set; }

    public User()
    {
    }

    public User(int id, string email, string token, string verificationToken, bool isVerified)
    {
        Id = id;
        Email = email;
        Token = token;
        VerificationToken = verificationToken;
        IsVerified = isVerified;
    }
}