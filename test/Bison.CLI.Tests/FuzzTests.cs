using System.Net;
using System.Net.Http.Json;

namespace Bison.CLI.Tests;

[Collection("Uses port 5090")]
public class FuzzTests
{
    
    // real taxon ids from joined.csv: Pelecaniformes, Ardeidae and Ardea cinerea
    private static readonly string[] ValidTaxonIds =
    {
        "MSTSNM:Arter:3e4e67e4-f785-ea11-aa77-501ac539d1ea", 
        "MSTSNM:Arter:495067e4-f785-ea11-aa77-501ac539d1ea", 
        "MSTSNM:Arter:c28811f4-f785-ea11-aa77-501ac539d1ea",
    };

    [Fact]
    public async Task RandomPosts_GiveExpectedResults()
    {
        using var service = new RunningService(Directory.CreateTempSubdirectory().FullName);
        var client = service.Client;
        var random = new Random(42); // same seed every time so we can rerun it if it fails

        //oracle
        var expectedObservations = new List<string>();
        var expectedComments = new Dictionary<int, List<string>>();
        var expectedProposals = new List<string>();

        for (int i = 0; i < 100; i++)
        {
            int kind = random.Next(3); 

            if (kind == 0 || expectedObservations.Count == 0)
            {
                string text = RandomText(random);
                var response = await client.PostAsJsonAsync("/observation", new ObservationRequest("fuzzer", text, "DR Byen"));

                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                expectedObservations.Add(text);
                continue;
            }

            bool validObservation = random.Next(10) < 8;
            int observationId = validObservation
                ? random.Next(1, expectedObservations.Count + 1)
                : random.Next(1000, 2000);

            if (kind == 1)
            {
                string text = RandomText(random);
                var response = await client.PostAsJsonAsync("/comment", new CommentRequest("fuzzer", text, observationId.ToString()));

                if (validObservation)
                {
                    Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                    if (!expectedComments.ContainsKey(observationId))
                    {
                        expectedComments[observationId] = new List<string>();
                    }
                    expectedComments[observationId].Add(text);
                }
                else
                {
                    Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
                }
            }
            else
            {
                bool validTaxon = random.Next(10) < 8;
                string taxonId = validTaxon
                    ? ValidTaxonIds[random.Next(ValidTaxonIds.Length)]
                    : "not-a-taxon-" + random.Next(1000);

                var response = await client.PostAsJsonAsync("/proposal", new ProposalRequest("fuzzer", taxonId, observationId));

                if (validObservation && validTaxon)
                {
                    Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                    expectedProposals.Add(taxonId);
                }
                else
                {
                    Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
                }
            }
        }

        var observations = await client.GetFromJsonAsync<List<ObserveCheep>>("/observations");
        Assert.Equal(expectedObservations, observations!.Select(o => o.Observation));

        foreach (var (observationId, comments) in expectedComments)
        {
            var actualComments = await client.GetFromJsonAsync<List<CommentCheep>>($"/comments/{observationId}");
            Assert.Equal(comments, actualComments!.Select(c => c.Comment));
        }

        var proposals = await client.GetFromJsonAsync<List<ProposalCheep>>("/proposals");
        Assert.Equal(expectedProposals, proposals!.Select(p => p.TaxonId));
    }
    private static string RandomText(Random random)
    {
        const string characters = "abcdefghijklmnopqrstuvwxyzæøå ,\"";
        int length = random.Next(1, 30);
        return new string(Enumerable.Range(0, length).Select(_ => characters[random.Next(characters.Length)]).ToArray());
    }
}