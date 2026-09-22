using SimpleDB;

string CSVObservePath = "../../bison_observe_cli_db.csv";
string CSVCommentPath = "../../bison_comment_cli_db.csv";
string CSVProposalPath = "../../bison_proposal_cli_db.csv";

CSVDatabase<ObserveCheep> dbObserve = CSVDatabase<ObserveCheep>.GetInstance(CSVObservePath);

CSVDatabase<CommentCheep> dbComment = CSVDatabase<CommentCheep>.GetInstance(CSVCommentPath);

CSVDatabase<ProposalCheep> dbProposal = CSVDatabase<ProposalCheep>.GetInstance(CSVProposalPath);

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

app.MapPost("/proposal", (ProposalRequest request) =>
{
    var proposalService = new ProposalService(dbProposal, dbObserve);

    if (!proposalService.TryAddProposal(
        request.Author, 
        request.TaxonId, 
        request.ObservationId, 
        out var error))
    {
    return Results.BadRequest(error);
    }
    return Results.Ok();
});

app.MapGet("/proposals", () =>
{
    return Results.Ok(dbProposal.Read());
});

app.Run();

