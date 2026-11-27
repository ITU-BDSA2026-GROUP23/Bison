using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Bison.Core.DTOs;
using Bison.Core.Interfaces;

namespace Bison.Razor.Pages;

public class SearchModel : PageModel
{
    private readonly IObservationService _service;
    public List <ObservationDTO> Observations {get; set; } = new();
    public string Query { get; set; } = "";

    public SearchModel(IObservationService service)
    {
        _service = service;
    }

    public IActionResult OnGet(string? q, int page = 1)
    {
        if (!string.IsNullOrWhiteSpace(q))
        {
            Query = q.Trim();
            Observations = _service.SearchObservations(Query, page);
        }

        return Page();
    }
}