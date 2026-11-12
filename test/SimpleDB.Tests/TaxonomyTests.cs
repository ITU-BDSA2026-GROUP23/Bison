namespace SimpleDB.Tests;

public class TaxonomyTests
{
    private const string ArdeidaeId = "MSTSNM:Arter:495067e4-f785-ea11-aa77-501ac539d1ea";

    [Fact]
    public void GetById_ReturnsTheRightTaxon()
    {
        var taxonomy = Taxonomy.Load();

        var taxon = taxonomy.GetById(ArdeidaeId);

        Assert.NotNull(taxon);
        Assert.Equal("Ardeidae", taxon.ScientificName);
    }

    [Fact]
    public void GetById_UnknownId_ReturnsNull()
    {
        var taxonomy = Taxonomy.Load();

        var taxon = taxonomy.GetById("not-a-taxon");

        Assert.Null(taxon);
    }

    [Fact]
    public void FindByDanishName_Fiskehejre_GivesArdeaCinerea()
    {
        var taxonomy = Taxonomy.Load();

        var taxon = taxonomy.GetByVernacularName("Fiskehejre");

        Assert.NotNull(taxon);
        Assert.Equal("Ardea cinerea", taxon.ScientificName);
    }

    [Fact]
    public void GetSuperTaxon_OfArdeidae_IsPelecaniformes()
    {
        var taxonomy = Taxonomy.Load();
        var ardeidae = taxonomy.GetById(ArdeidaeId)!;

        var superTaxon = taxonomy.getSuperTaxon(ardeidae);

        Assert.NotNull(superTaxon);
        Assert.Equal("Pelecaniformes", superTaxon.ScientificName);
    }

    [Fact]
    public void GetSubtaxons_OfArdeidae_ContainsArdea()
    {
        var taxonomy = Taxonomy.Load();
        var ardeidae = taxonomy.GetById(ArdeidaeId)!;

        var subtaxons = taxonomy.GetSubtaxons(ardeidae);

        Assert.Contains(subtaxons, t => t.ScientificName == "Ardea");
    }
}