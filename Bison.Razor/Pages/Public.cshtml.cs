using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Bison.Razor.DTOs;

namespace Bison.Razor.Pages;

public class PublicModel : PageModel
{
    private readonly IObservationService _service;
    public List<ObservationDTO> Observations { get; set; }

    public PublicModel(IObservationService service)
    {
        _service = service;
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
}
