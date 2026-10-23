using Models;
public interface IPostRepository
{
    List <Observation> GetObservations(int page);
    List <Observation> GetObservationsFromAuthor(string author, int page);
    List <Observation> GetObservationsFromId(int id, int page);
    List <Comment> GetCommentsFromObservation(int observationId);
    List <Proposal> GetProposalsFromObservation(int observationId);
}