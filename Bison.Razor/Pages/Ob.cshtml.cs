using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Bison.Razor.Pages;

public class ObModel : PageModel
{
    private readonly IObservationService _service;
    public List<ObservationViewModel> Observations { get; set; }

    public ObModel(IObservationService service)
    {
        _service = service;
    }

    public ActionResult OnGet(string id, [FromQuery] int page = 1)
    {
        
        if (!string.IsNullOrEmpty(id))
        {
            Observations = _service.GetObservationsFromId(id, page);
            return Page();
        }
        
        Observations = _service.GetObservations(page);
        return Page();
    }
}
