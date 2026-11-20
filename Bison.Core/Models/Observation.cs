namespace Models;
public class Observation : Post
{
    public Taxon? Taxon { get; set; }
    public List<Comment> Comments { get; set; } = new();
    public List<Proposal> Proposals { get; set; } = new();
    public byte[]? ImageData { get; set; }
    public string? ImageContentType { get; set; }
    
    public Taxon getTaxon()
    {
        return Taxon;
    }
}