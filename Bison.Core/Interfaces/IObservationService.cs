using Bison.Core.DTOs;

namespace Bison.Core.Interfaces;

public interface IObservationService
{
    public List<ObservationDTO> GetObservations(int page);
    public List<ObservationDTO> GetObservationsFromAuthor(string author, int page);
    public List<ObservationDTO> GetObservationsFromId(int id, int page);
    public List<CommentDTO> GetCommentsFromObservation(int id);
    public List<ProposalDTO> GetProposalsFromObservation(int id);
    public List<ObservationDTO> GetObservationsByTaxon(int taxonID, int page);
}