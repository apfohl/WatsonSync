namespace WatsonSync.Models;

public sealed record User
{
    public User(int id, string email, string token)
    {
        Id = id;
        Email = email;
        Token = token;
    }

    public User()
    {
    }

    public int Id { get; set; }
    public string Email { get; set; }
    public string Token { get; set; }
}