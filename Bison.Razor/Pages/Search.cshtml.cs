using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Bison.Core.DTOs;
using Bison.Core.Interfaces;

namespace Bison.Razor.Pages;

public class SearchModel : PageModel
{
    private readonly IObservationService _service;
    private readonly IUserService _userService;

    public List<ObservationDTO> Observations { get; set; } = new();
    public List<string> Users { get; set; } = new();

    public string Query { get; set; } = "";

    public SearchModel(
        IObservationService service,
        IUserService userService)
    {
        _service = service;
        _userService = userService;
    }

    public IActionResult OnGet(string? q, int page = 1)
    {
        if (!string.IsNullOrWhiteSpace(q))
        {
            Query = q.Trim();
            Observations = _service.SearchObservations(Query, page);
            Users = _userService.SearchUsers(Query);
        }

        return Page();
    }
}