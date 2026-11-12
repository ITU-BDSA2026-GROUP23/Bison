using SimpleDB;

public class ProposalService
{
    private readonly CSVDatabase<ProposalCheep> dbProposal;
    private readonly CSVDatabase<ObserveCheep> dbObserve;
    private static readonly Taxonomy taxonomy = Taxonomy.Load();
    public ProposalService(CSVDatabase<ProposalCheep> dbProposal, CSVDatabase<ObserveCheep> dbObserve)
    {
        this.dbProposal = dbProposal;
        this.dbObserve = dbObserve;
    }

    public bool TryAddProposal(string author, string taxonId, int observationId, out string? error)
    {
        bool observationExists = dbObserve.Read().Any(o => o.id == observationId);
        if (!observationExists)
        {
            error = $"Observation {observationId} does not exist.";
            return false;
        }

        bool taxonExists = taxonomy.GetById(taxonId) != null;
        if (!taxonExists)
        {
            error = $"Taxon {taxonId} does not exist.";
            return false;
        }
    
        var proposal = new ProposalCheep(author, taxonId, DateTimeOffset.UtcNow.ToUnixTimeSeconds(), observationId);
        dbProposal.Store(proposal);
        error = null;
        return true;
    }
}