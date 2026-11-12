using Models;

namespace Bison.Razor.Tests;

public class ObservationServiceTests
{
    // fake repository so only the service is tested
    private class PostRepositoryStub : IPostRepository
    {
        public List<Observation> Observations = new();
        public List<Proposal> Proposals = new();

        public List<Observation> GetObservations(int page) => Observations;
        public List<Observation> GetObservationsFromAuthor(string author, int page) => Observations;
        public List<Observation> GetObservationsFromId(int id, int page) => Observations;
        public List<Comment> GetCommentsFromObservation(int observationId) => new();
        public List<Proposal> GetProposalsFromObservation(int observationId) => Proposals;
        public List<Observation> GetAllObservations() => Observations;
    }

    [Fact]
    public void GetObservations_GivesAuthorAndText()
    {
        var stub = new PostRepositoryStub();
        stub.Observations.Add(new Observation { Author = new Author { Name = "Peter" }, Text = "A big gray bird in a pond at DR byen", TimeStamp = DateTime.Now });
        var service = new ObservationService(stub, new TaxonFilterService());

        var result = service.GetObservations(1);

        Assert.Single(result);
        Assert.Equal("Peter", result[0].Author);
        Assert.Equal("A big gray bird in a pond at DR byen", result[0].Text);
    }

    [Fact]
    public void Proposal_WithTaxon_ShowsDanishName()
    {
        var stub = new PostRepositoryStub();
        stub.Proposals.Add(new Proposal { Author = new Author { Name = "Petra" }, Text = "A heron", TimeStamp = DateTime.Now, Taxon = new Taxon { VernacularName = "Fiskehejre" } });
        var service = new ObservationService(stub, new TaxonFilterService());

        var result = service.GetProposalsFromObservation(1);

        Assert.Equal("Fiskehejre", result[0].TaxonName);
    }

    [Fact]
    public void Proposal_WithoutTaxon_SaysUnknownTaxon()
    {
        var stub = new PostRepositoryStub();
        stub.Proposals.Add(new Proposal { Author = new Author { Name = "Petra" }, Text = "A heron", TimeStamp = DateTime.Now });
        var service = new ObservationService(stub, new TaxonFilterService());

        var result = service.GetProposalsFromObservation(1);

        Assert.Equal("Unknown taxon", result[0].TaxonName);
    }

    [Fact]
    public void GetObservationsByTaxon_UnknownTaxon_ReturnsEmpty()
    {
        var stub = new PostRepositoryStub();
        stub.Observations.Add(new Observation { Author = new Author { Name = "Peter" }, Text = "A bird", TimeStamp = DateTime.Now, Taxon = new Taxon { TaxonId = 1 } });
        var service = new ObservationService(stub, new TaxonFilterService());

        var result = service.GetObservationsByTaxon(999, 1);

        Assert.Empty(result);
    }
}