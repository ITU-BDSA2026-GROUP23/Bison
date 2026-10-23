using Models;
public class Comment : Post
{
    public Observation Observation { get; set; } = null!;
}