using System.Text.Json;
using LE.CS.Api;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<IDocumentStore, JsonlDocumentStore>();

var app = builder.Build();

app.MapPut("/{collection}/{id}", async (string collection, string id, HttpRequest req, IDocumentStore store) =>
{
    if (!Names.Valid(collection) || !Names.Valid(id)) return Results.BadRequest();
    try
    {
        var doc = await JsonSerializer.DeserializeAsync<JsonElement>(req.Body);
        return Results.Json(new { version = store.Save(collection, id, doc) });
    }
    catch (JsonException)
    {
        return Results.BadRequest();
    }
});

app.MapGet("/{collection}/{id}", (string collection, string id, IDocumentStore store) =>
    !Names.Valid(collection) || !Names.Valid(id) ? Results.BadRequest()
    : store.Get(collection, id) is { } json ? Results.Content(json, "application/json") : Results.NotFound());

app.MapGet("/{collection}", (string collection, IDocumentStore store) =>
    Names.Valid(collection) ? Results.Json(store.List(collection)) : Results.BadRequest());

app.Run();

public partial class Program { }
