using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Bison.Core.DTOs;
using Bison.Core.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;

namespace Bison.Razor.Pages;

public class PublicModel : PageModel
{
    private readonly IObservationService _service;
    private readonly UserManager<IdentityUser> _userManager;

    public List<ObservationDTO> Observations { get; set; } = new();

    public PublicModel(IObservationService service, UserManager<IdentityUser> userManager)
        {
            _service = service;
            _userManager = userManager;
        }

    public ActionResult OnGet([FromQuery] int page = 1,[FromQuery] int? taxonId = null, [FromQuery] string? q = null)
    {
        if (!string.IsNullOrWhiteSpace(q))
            Observations = _service.SearchObservations(q, page);
        else if (taxonId.HasValue)
            Observations = _service.GetObservationsByTaxon(taxonId.Value, page);
        else
            Observations = _service.GetObservations(page);

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(string text, IFormFile? image)
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

        byte[]? imageData = null;
        string? imageContentType = null;

        if (image != null)
        {
            if (image.Length == 0 || image.Length > 5 * 1024 * 1024)
                return BadRequest("Image must be between 1 byte and 5 MB.");

            using var stream = image.OpenReadStream();

            using var memoryStream = new MemoryStream();
            var buffer = new byte[81920];
            int bytesRead;
            while ((bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length)) > 0)
            {
                if (memoryStream.Length + bytesRead > 5 * 1024 * 1024)
                    return BadRequest("Image must be between 1 byte and 5 MB.");

                memoryStream.Write(buffer, 0, bytesRead);
            }

            imageData = memoryStream.ToArray();

            bool jpeg = imageData.Length >= 3
                && imageData[0] == 0xFF
                && imageData[1] == 0xD8
                && imageData[2] == 0xFF;

            bool png = imageData.Length >= 8
                && imageData[..8].SequenceEqual(new byte[] { 137, 80, 78, 71, 13, 10, 26, 10 });

            bool webp = imageData.Length >= 12
                && System.Text.Encoding.ASCII.GetString(imageData, 0, 4) == "RIFF"
                && System.Text.Encoding.ASCII.GetString(imageData, 8, 4) == "WEBP";

            if (!jpeg && !png && !webp)
            {
                return BadRequest("Image must be in JPEG, PNG, or WebP format.");
            }

            imageContentType = jpeg ? "image/jpeg" : png ? "image/png" : "image/webp";
        }
        _service.AddObservation(
            text,
            user.Id,
            user.Email,
            imageData,
            imageContentType);

        return RedirectToPage("/Public");
    }
}
