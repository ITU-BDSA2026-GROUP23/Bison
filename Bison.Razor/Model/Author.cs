public class Author
{
    public int AuthorId { get; set; }
    public String Name { get; set; } = "";
    public string Email { get; set; } = "";
    public List<Post> Posts { get; set; } = new();
}