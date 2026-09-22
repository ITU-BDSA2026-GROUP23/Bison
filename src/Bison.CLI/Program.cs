using System.Net.Http.Headers;
using System.Net.Http.Json;

var baseURL = "http://localhost:5090";
using HttpClient client = new();
client.DefaultRequestHeaders.Accept.Clear();
client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue ("application/json"));
client.BaseAddress = new Uri(baseURL);

UserInterface ui = new UserInterface();

if (args.Length == 0)
{
    Console.WriteLine("Enter read, observe, comment or discussion");
    return;
}
if (args[0] == "read")
{
    var observations = await client.GetFromJsonAsync<List<ObserveCheep>>("observations");
    ui.PrintObservations(observations);
}
else if (args[0] == "observe")
{
    var observation = new ObservationRequest(Environment.UserName, args[1], args[2]);
    await client.PostAsJsonAsync("observation",observation);
}
else if (args[0] == "comment") //Now we print a clear message and refuse to save when the id doesn't exist.
{
    var comment = new CommentRequest(Environment.UserName, args[1], args[2]);
    await client.PostAsJsonAsync("comment", comment);
}
else if (args[0] == "discussion")
{
    var comments = await client.GetFromJsonAsync<List<CommentCheep>>($"comments/{args[1]}");
    ui.PrintComments(comments);
}
else
{
    Console.WriteLine("Invalid command. Use 'read', 'observe', 'discussion', 'location' or 'comment'.");
}

public record ObservationRequest(string Author, string Observation, string Location);
public record CommentRequest(string Author, string Comment, string ObservationId);

public record ObserveCheep(string Author, string Observation, long Timestamp, int id, string Location = "Unknown");
public record CommentCheep(string Author, string Comment, long Timestamp, int id);

