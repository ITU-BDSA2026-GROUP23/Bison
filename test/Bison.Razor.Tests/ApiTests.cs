using Microsoft.AspNetCore.Mvc.Testing;

namespace Bison.Razor.Tests;

public class ApiTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient client;

    public ApiTests(WebApplicationFactory<Program> factory)
    {
        // own db file so the tests dont touch the real one
        Environment.SetEnvironmentVariable("BISONDBPATH", Path.Combine(Path.GetTempPath(), $"bison_test_{Guid.NewGuid()}.db"));
        client = factory.CreateClient();
    }

    private async Task<string> GetPage(string url)
    {
        var response = await client.GetAsync(url);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
    }

    [Fact]
    public async Task FrontPage_ShowsNewestObservation()
    {
        var content = await GetPage("/");

        Assert.Contains("Nathan Sirmon", content);
        Assert.Contains("About 30 spoonbills at the lagoon, seen through the scope.", content);
    }

    [Fact]
    public async Task ObsPage_ShowsNewestObservation()
    {
        var content = await GetPage("/obs");

        Assert.Contains("Nathan Sirmon", content);
        Assert.Contains("About 30 spoonbills at the lagoon, seen through the scope.", content);
    }

    [Fact]
    public async Task UserPage_ShowsTheirOwnObservation()
    {
        var content = await GetPage("/obs/Roger%20Histand");

        Assert.Contains("Three little egrets resting near the salt marsh.", content);
    }

    [Fact]
    public async Task UserPage_HidesOtherUsersObservations()
    {
        var content = await GetPage("/obs/Roger%20Histand");

        Assert.DoesNotContain("About 30 spoonbills at the lagoon, seen through the scope.", content);
    }

    [Fact]
    public async Task ObsPage_WithPageOne_IsSameAsWithoutPage()
    {
        var withPage = await GetPage("/obs?page=1");
        var withoutPage = await GetPage("/obs");

        Assert.Equal(withoutPage, withPage);
    }

    [Fact]
    public async Task UserPage_WithPageOne_IsSameAsWithoutPage()
    {
        var withPage = await GetPage("/obs/Roger%20Histand?page=1");
        var withoutPage = await GetPage("/obs/Roger%20Histand");

        Assert.Equal(withoutPage, withPage);
    }
}