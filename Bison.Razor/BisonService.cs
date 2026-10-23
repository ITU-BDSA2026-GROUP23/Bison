using Bison.Razor.DTOs;
using Models;

public interface IObservationService
{
    public List<ObservationDTO> GetObservations(int page);
    public List<ObservationDTO> GetObservationsFromAuthor(string author, int page);
    public List<ObservationDTO> GetObservationsFromId(int id, int page);
    public List<CommentDTO> GetCommentsFromObservation(int id);
    public List<ProposalDTO> GetProposalsFromObservation(int id);
    public List<ObservationDTO> GetObservationsByTaxon(int taxonID, int page);
}


public class ObservationService : IObservationService
{
    private readonly IPostRepository _postRepository;
    private readonly ITaxonFilterService _taxonFilterService;

    public ObservationService(IPostRepository postRepository, ITaxonFilterService taxonFilterService)
    {
        _postRepository = postRepository;
        _taxonFilterService = taxonFilterService;
    }


    public List<ObservationDTO> GetObservationsByTaxon(int taxonId, int page)
    {
        var observations = _postRepository.GetAllObservations();

        Taxon? root = null;

        foreach (var observation in observations)
        {
            Taxon? current = observation.Taxon;
            while (current != null)
            {
                if (current.TaxonId == taxonId)
                {
                    root = current;
                    break;
                }

                current = current.Parent;
            }
            if (root != null)
                break;
        }

        if (root == null)
            return new List<ObservationDTO>();


        var filtered = _taxonFilterService.FilterByTaxon(root, observations);

        var paginated = filtered.Skip((Math.Max(1, page) - 1) * 32).Take(32).ToList();

        return paginated.Select(o => new ObservationDTO(o.Author.Name,o.Text,o.TimeStamp.ToString("MM/dd/yy H:mm:ss"))).ToList();
    }
    public List<ObservationDTO> GetObservations(int page)
    {
        var observations = _postRepository.GetObservations(page);
        return observations.Select(o => new ObservationDTO(o.Author.Name, o.Text, o.TimeStamp.ToString("MM/dd/yy H:mm:ss"))).ToList();
    }

    public List<ObservationDTO> GetObservationsFromAuthor(string author, int page)
    {
        var observations = _postRepository.GetObservationsFromAuthor(author, page);
        return observations.Select(o => new ObservationDTO(o.Author.Name, o.Text, o.TimeStamp.ToString("MM/dd/yy H:mm:ss"))).ToList();
    }   

    public List<ObservationDTO> GetObservationsFromId(int id, int page)
    {
        var observations = _postRepository.GetObservationsFromId(id, page);
        return observations.Select(o => new ObservationDTO(o.Author.Name, o.Text, o.TimeStamp.ToString("MM/dd/yy H:mm:ss"))).ToList();
    }

    public List<CommentDTO> GetCommentsFromObservation(int id)
    {
        var comments = _postRepository.GetCommentsFromObservation(id);
        return comments.Select(c => new CommentDTO(c.Author.Name, c.Text, c.TimeStamp.ToString("MM/dd/yy H:mm:ss"))).ToList();
    }

    public List<ProposalDTO> GetProposalsFromObservation(int id)
    {
        var proposals = _postRepository.GetProposalsFromObservation(id);
        return proposals.Select(p => new ProposalDTO(p.Author.Name, p.Text, p.TimeStamp.ToString("MM/dd/yy H:mm:ss"), p.Taxon?.VernacularName ?? p.Taxon?.dwc_TaxonID ?? "Unknown taxon")).ToList();
    }

    private static string UnixTimeStampToDateTimeString(double unixTimeStamp)
    {
        // Unix timestamp is seconds past epoch
        DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        dateTime = dateTime.AddSeconds(unixTimeStamp);
        return dateTime.ToString("MM/dd/yy H:mm:ss");
    }

}
