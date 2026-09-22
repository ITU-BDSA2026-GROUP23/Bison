using CsvHelper;
using System.Globalization;
using System.Reflection;

namespace SimpleDB;
public record Taxonomy
{
    private List <Taxon> taxons;

    public Taxonomy(List<Taxon> taxons)
    {
        this.taxons = taxons;
    }

    public Taxon? GetById(string id)
    {
        return taxons.FirstOrDefault(t => t.TaxonID == id);
    }

    public Taxon? GetByVernacularName(string name)
    {
        return taxons.FirstOrDefault(t => t.VernacularName == name);
    }

    public Taxon? getSuperTaxon(Taxon taxon)
    {
        return GetById(taxon.ParentNameUsageID);
    }

    public IEnumerable<Taxon> GetSubtaxons(Taxon taxon)
    {
        return taxons.Where(t => t.ParentNameUsageID == taxon.TaxonID);
    }

    public static Taxonomy Load()
    {
        var assembly = Assembly.GetExecutingAssembly();

        var stream = assembly.GetManifestResourceStream("SimpleDB.joined.csv");

        using var reader = new StreamReader(stream!);
        using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

        csv.Read();
        csv.ReadHeader();

        var taxons = new List<Taxon>();

        while (csv.Read())
        {
            var taxon = new Taxon(
                csv.GetField("dwc:taxonID")!,
                csv.GetField("dwc:parentNameUsageID")!,
                csv.GetField("dwc:acceptedNameUsageID")!,
                csv.GetField("dwc:taxonomicStatus")!,
                csv.GetField("dwc:taxonRank")!,
                csv.GetField("dwc:scientificName")!,
                csv.GetField("dwc:scientificNameAuthorship")!,
                csv.GetField("dcterms:language")!,
                csv.GetField("dwc:vernacularName")!,
                csv.GetField("clb:merged") == "true"
            );

            taxons.Add(taxon);
        }

        return new Taxonomy(taxons);
    }
}