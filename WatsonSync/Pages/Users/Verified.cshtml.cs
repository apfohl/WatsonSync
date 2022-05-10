using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WatsonSync.Models;

namespace WatsonSync.Pages.Users;

public sealed class Verified : PageModel
{
    [BindProperty]
    public string ApiKey { get; private set; }
    
    public IActionResult OnGet(Token token)
    {
        ApiKey = token.Value;
        return Page();
    }
}