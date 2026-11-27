using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Bison.Core.DTOs;
using Bison.Core.Interfaces;

namespace Bison.Razor.Pages;

[Authorize]
public class AboutMeModel : PageModel
{
    private readonly IObservationService _service;
    private readonly UserManager<IdentityUser> _userManager;

    public string? Username { get; set; }
    public string? Email { get; set; }

    public AuthorDTO? AuthorInfo { get; set; }

    public List<ObservationDTO> Observations { get; set; } = new();
    public List<CommentDTO> Comments { get; set; } = new();
    public List<ProposalDTO> Proposals { get; set; } = new();

    public AboutMeModel(
        IObservationService service,
        UserManager<IdentityUser> userManager)
    {
        _service = service;
        _userManager = userManager;
    }

    public async Task<IActionResult> OnGetAsync()
    {
        var user = await _userManager.GetUserAsync(User);

        if (user == null)
            return Forbid();

        Username = user.UserName;
        Email = user.Email;

        var userPosts = _service.GetPostsFromIdentityUser(user.Id);

        AuthorInfo = userPosts.Author;
        Observations = userPosts.Observations;
        Comments = userPosts.Comments;
        Proposals = userPosts.Proposals;

        return Page();
    }
}