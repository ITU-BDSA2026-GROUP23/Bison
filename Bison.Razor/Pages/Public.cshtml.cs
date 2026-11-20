using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Bison.Core.DTOs;
using Bison.Core.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Bison.Razor.Pages;

public class PublicModel : PageModel
{
    private readonly IObservationService _service;
    private readonly UserManager<IdentityUser> _userManager;

    public List<ObservationDTO> Observations { get; set; }

    public PublicModel(IObservationService service, UserManager<IdentityUser> userManager)
        {
            _service = service;
            _userManager = userManager;
        }

    public ActionResult OnGet([FromQuery] int page = 1,[FromQuery] int? taxonId = null)
    {
        if (taxonId.HasValue)
        {
            Observations = _service.GetObservationsByTaxon(taxonId.Value, page);
        }
        else
        {
            Observations = _service.GetObservations(page);
        }

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(string text)
{
    if (User.Identity?.IsAuthenticated != true){
        return Challenge();
    }

    if (string.IsNullOrWhiteSpace(text) || text.Length > 2000){
        return BadRequest("Invalid observation text.");
    }

    var user = await _userManager.GetUserAsync(User);

    if (user == null || string.IsNullOrWhiteSpace(user.Email)){
        return Forbid();
    }

    _service.AddObservation(text, user.Id, user.Email);

    return RedirectToPage("/Public");
}

    
}
