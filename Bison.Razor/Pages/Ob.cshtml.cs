using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Bison.Core.DTOs;
using Bison.Core.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Bison.Razor.Pages;

public class ObModel : PageModel
{
    private readonly IObservationService _service;
    private readonly UserManager<IdentityUser> _userManager;
    public List<ObservationDTO> Observations { get; set; }
    public List<CommentDTO> Comments { get; set; } = new();
    public List<ProposalDTO> Proposals { get; set; } = new();

    public ObModel(IObservationService service, UserManager<IdentityUser> userManager)
    {
        _service = service;
        _userManager = userManager;
    }

    public ActionResult OnGet(string id, [FromQuery] int page = 1)
    {
        
        if (!string.IsNullOrEmpty(id))
        {
            Observations = _service.GetObservationsFromId(int.Parse(id), page);
            Comments = _service.GetCommentsFromObservation(int.Parse(id));
            Proposals = _service.GetProposalsFromObservation(int.Parse(id));
            return Page();
        }
        
        Observations = _service.GetObservations(page);
        return Page();
    }

    public async Task<IActionResult> OnPostCommentAsync(int observationId, string text)
    {
        if (User.Identity?.IsAuthenticated != true)
            return Challenge();

        if (observationId <= 0 || string.IsNullOrWhiteSpace(text) || text.Length > 2000)
            return BadRequest("Invalid comment.");

        var user = await _userManager.GetUserAsync(User);

        if (user == null || string.IsNullOrWhiteSpace(user.Email))
            return Forbid();

        _service.AddComment(observationId, text, user.Id, user.Email);

        return RedirectToPage("/Ob", new { id = observationId });
    }

    public async Task<IActionResult> OnPostProposalAsync(int observationId, int taxonId, string text)
    {
        if (User.Identity?.IsAuthenticated != true)
            return Challenge();

        if (observationId <= 0 || taxonId <= 0 ||
            string.IsNullOrWhiteSpace(text) || text.Length > 2000)
            return BadRequest("Invalid proposal.");

        var user = await _userManager.GetUserAsync(User);

        if (user == null || string.IsNullOrWhiteSpace(user.Email))
            return Forbid();

        _service.AddProposal(observationId, taxonId, text, user.Id, user.Email);

        return RedirectToPage("/Ob", new { id = observationId });
    }
}
