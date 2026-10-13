public interface IPostRepository
{
    List <Observation> GetObservations(int page);
    List <Observation> GetObservationsFromAuthor(string author, int page);
    List <Observation> GetObservationsFromId(int id, int page);
}