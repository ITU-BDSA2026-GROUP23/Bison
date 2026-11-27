using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;

namespace Bison.Razor.Pages;

[Authorize]
public class AboutMeModel : PageModel
{
    public string? Username { get; set; }
    public string? Email { get; set; }

    public void OnGet()
    {
        Username = User.Identity?.Name;
        Email = User.FindFirst(
            System.Security.Claims.ClaimTypes.Email
        )?.Value;

        // Retrieve the user's observations,
        // comments and proposals here.
    }
}