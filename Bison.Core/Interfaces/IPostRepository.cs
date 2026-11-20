using Models;

namespace Bison.Core.Interfaces;

public interface IPostRepository
{
    List <Observation> GetObservations(int page);
    List <Observation> GetObservationsFromAuthor(string author, int page);
    List <Observation> GetObservationsFromId(int id, int page);
    List <Comment> GetCommentsFromObservation(int observationId);
    List <Proposal> GetProposalsFromObservation(int observationId);
    List <Observation> GetAllObservations();
    void AddObservation(
        string text,
        string identityUserID,
        string email,
        byte[]? imageData,
        string? imageContentType);

    void AddComment(
        int observationId,
        string text,
        string identityUserID,
        string email);

    void AddProposal(
        int observationId,
        int taxonId,
        string text,
        string identityUserID,
        string email);
}