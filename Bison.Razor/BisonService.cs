using Bison.Razor.DTOs;

public interface IObservationService
{
    public List<ObservationDTO> GetObservations(int page);
    public List<ObservationDTO> GetObservationsFromAuthor(string author, int page);

    public List<ObservationDTO> GetObservationsFromId(int id, int page);
}


public class ObservationService : IObservationService
{
    private readonly IPostRepository _postRepository;
    public ObservationService(IPostRepository postRepository)
    {
        _postRepository = postRepository;
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

    private static string UnixTimeStampToDateTimeString(double unixTimeStamp)
    {
        // Unix timestamp is seconds past epoch
        DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        dateTime = dateTime.AddSeconds(unixTimeStamp);
        return dateTime.ToString("MM/dd/yy H:mm:ss");
    }

}
