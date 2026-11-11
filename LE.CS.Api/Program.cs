using System.Text.Json;
using LE.CS.Api;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<IDocumentStore, JsonlDocumentStore>();

var app = builder.Build();

app.MapPut("/{collection}/{id}", async (string collection, string id, HttpRequest req, IDocumentStore store) =>
{
    var doc = await JsonSerializer.DeserializeAsync<JsonElement>(req.Body);
    return Results.Json(new { version = store.Save(collection, id, doc) });
});

app.MapGet("/{collection}/{id}", (string collection, string id, IDocumentStore store) =>
    store.Get(collection, id) is { } json ? Results.Content(json, "application/json") : Results.NotFound());

app.Run();

public partial class Program { }
