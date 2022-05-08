using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WatsonSync.Components.Verification;
using WatsonSync.Models;

namespace WatsonSync.Pages.Users;

public sealed class Verification : PageModel
{
    private readonly UserVerifier userVerifier;

    [BindProperty] public string Email { get; set; }

    [BindProperty] public string VerificationToken { get; set; }

    public Verification(UserVerifier userVerifier) =>
        this.userVerifier = userVerifier;

    public void OnGet(string email, string verificationToken)
    {
        Email = email;
        VerificationToken = verificationToken;
    }
    
    public async Task<IActionResult> OnPostAsync() =>
        (await userVerifier.Verify(new UserVerification(Email, VerificationToken)))
        .Match(
            error => error switch
            {
                VerificationError.NotFound => NotFound(),
                VerificationError.Unauthorized => (IActionResult)Unauthorized(),
                _ => throw new ArgumentOutOfRangeException(nameof(error), error, null)
            },
            token => new CreatedResult(string.Empty, token));
}