public record ObservationViewModel(string Author, string Message, string Timestamp);

public interface IObservationService
{
    public List<ObservationViewModel> GetObservations(int page);
    public List<ObservationViewModel> GetObservationsFromAuthor(string author, int page);

    public List<ObservationViewModel> GetObservationsFromId(string id, int page);
}


public class ObservationService : IObservationService
{
    private readonly IPostRepository _postRepository;
    public ObservationService(IPostRepository postRepository)
    {
        _postRepository = postRepository;
    }

    public List<ObservationViewModel> GetObservations(int page)
    {
        return _postRepository.GetObservations(page);
    }

    public List<ObservationViewModel> GetObservationsFromAuthor(string author, int page)
    {
        return _postRepository.GetObservationsFromAuthor(author, page);
    }

    public List<ObservationViewModel> GetObservationsFromId(string id, int page)
    {
        return _postRepository.GetObservationsFromId(id, page);
    }

}
