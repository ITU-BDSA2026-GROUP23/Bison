public interface IPostRepository
{
    List <ObservationViewModel> GetObservations(int page);
    List <ObservationViewModel> GetObservationsFromAuthor(string author, int page);
    List <ObservationViewModel> GetObservationsFromId(string id, int page);
}