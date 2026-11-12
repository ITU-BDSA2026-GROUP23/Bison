using System.Net;
using System.Net.Http.Json;

namespace Bison.CLI.Tests;

[Collection("Uses port 5090")]
public class CSVDBServiceTests
{
    private const string ArdeaCinereaId = "MSTSNM:Arter:c28811f4-f785-ea11-aa77-501ac539d1ea";

    [Fact]
    public async Task GetObservations_ReturnsOkAndList()
    {
        using var service = new RunningService(Directory.CreateTempSubdirectory().FullName);
        await service.Client.PostAsJsonAsync("/observation", new ObservationRequest("ropf", "A bird at DR Byen", "DR Byen"));

        var response = await service.Client.GetAsync("/observations");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var observations = await response.Content.ReadFromJsonAsync<List<ObserveCheep>>();
        Assert.NotNull(observations);
        Assert.Single(observations);
        Assert.Equal("ropf", observations[0].Author);
        Assert.Equal("A bird at DR Byen", observations[0].Observation);
    }

    [Fact]
    public async Task PostObservation_ReturnsOk()
    {
        using var service = new RunningService(Directory.CreateTempSubdirectory().FullName);

        var response = await service.Client.PostAsJsonAsync("/observation", new ObservationRequest("emili", "Heron", "DR Byen"));

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task Comment_OnExistingObservation_CanBeReadBack()
    {
        using var service = new RunningService(Directory.CreateTempSubdirectory().FullName);
        await service.Client.PostAsJsonAsync("/observation", new ObservationRequest("emili", "Heron", "DR Byen"));

        var response = await service.Client.PostAsJsonAsync("/comment", new CommentRequest("nikolai", "nice bird", "1"));
        var comments = await service.Client.GetFromJsonAsync<List<CommentCheep>>("/comments/1");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(comments);
        Assert.Single(comments);
        Assert.Equal("nice bird", comments[0].Comment);
    }

    [Fact]
    public async Task Comment_OnNonExistingObservation_IsRejected()
    {
        using var service = new RunningService(Directory.CreateTempSubdirectory().FullName);

        var response = await service.Client.PostAsJsonAsync("/comment", new CommentRequest("nikolai", "nice bird", "99999"));

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Proposal_WithValidTaxon_CanBeReadBack()
    {
        using var service = new RunningService(Directory.CreateTempSubdirectory().FullName);
        await service.Client.PostAsJsonAsync("/observation", new ObservationRequest("emili", "Heron", "DR Byen"));

        var response = await service.Client.PostAsJsonAsync("/proposal", new ProposalRequest("patrick", ArdeaCinereaId, 1));
        var proposals = await service.Client.GetFromJsonAsync<List<ProposalCheep>>("/proposals");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(proposals);
        Assert.Single(proposals);
        Assert.Equal(ArdeaCinereaId, proposals[0].TaxonId);
    }

    [Fact]
    public async Task Proposal_WithInvalidTaxon_IsRejected()
    {
        using var service = new RunningService(Directory.CreateTempSubdirectory().FullName);
        await service.Client.PostAsJsonAsync("/observation", new ObservationRequest("emili", "Heron", "DR Byen"));

        var response = await service.Client.PostAsJsonAsync("/proposal", new ProposalRequest("patrick", "not-a-taxon", 1));

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
}
