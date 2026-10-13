public class Observation : Post
{
    public Taxon Taxon { get; set; }
    public List<Comment> Comments { get; set; }
    public List<Proposal> Proposals { get; set; }
}