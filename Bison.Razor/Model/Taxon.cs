using System.Collections.Generic;

public class Taxon
{
    public string dwc_TaxonID { get; set; } = "";
    public string DanishVernacularName { get; set; } = "";
    public Taxon? Parent { get; set; }
    public List<Taxon> Children { get; set; } = new();
}