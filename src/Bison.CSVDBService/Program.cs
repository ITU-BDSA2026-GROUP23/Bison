using SimpleDB;

string CSVObservePath = "../../bison_observe_cli_db.csv";
string CSVCommentPath = "../../bison_comment_cli_db.csv";

CSVDatabase<ObserveCheep> dbObserve = CSVDatabase<ObserveCheep>.GetInstance(CSVObservePath);

CSVDatabase<CommentCheep> dbComment = CSVDatabase<CommentCheep>.GetInstance(CSVCommentPath);

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapPost("/observation", (ObservationRequest request) =>
{
    var id = IdGenerator.NextObserveId(CSVObservePath);

    var observation = new ObserveCheep(
        request.Author,
        request.Observation,
        DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
        id,
        request.Location);

    dbObserve.Store(observation);

    return Results.Ok(observation);
});

app.MapPost("/comment", (CommentRequest request) =>
{
    var observationId = int.Parse(request.ObservationId);

    var commentService = new CommentService(dbObserve, dbComment);

    if (!commentService.TryAddComment(
            request.Author,
            request.Comment,
            observationId,
            out var error))
    {
        return Results.BadRequest(error);
    }

    return Results.Ok();
});

app.MapGet("/observations", () =>
{
    return Results.Ok(dbObserve.Read());
});

app.MapGet("/comments/{observationId}", (string observationId) =>
{
    var id = int.Parse(observationId);

    return Results.Ok(dbComment.discussion(id));
});

app.Run();

public record ObservationRequest(string Author, string Observation, string Location);

public record CommentRequest(string Author, string Comment, string ObservationId);