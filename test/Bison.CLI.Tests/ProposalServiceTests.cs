using SimpleDB;

namespace Bison.CLI.Tests;

public class ProposalServiceTests
{
    [Fact]
    public void TryAddProposal_OnNonExistingObservation_IsRejected()
    {
        string observePath = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".csv");
        string proposalPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".csv");

        var dbObserve = CSVDatabase<ObserveCheep>.GetInstance(observePath);
        var dbProposal = CSVDatabase<ProposalCheep>.GetInstance(proposalPath);

        var proposalService = new ProposalService(dbProposal, dbObserve);

        bool result = proposalService.TryAddProposal("emili", "MSTSNM:Arter:c28811f4-f785-ea11-aa77-501ac539d1ea", 99999, out var error);

        Assert.False(result);
        Assert.NotNull(error);
    }
}