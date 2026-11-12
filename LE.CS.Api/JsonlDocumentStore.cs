using System.Text.Encodings.Web;
using System.Text.Json;

namespace LE.CS.Api;

public sealed class JsonlDocumentStore(IConfiguration config) : IDocumentStore
{
    static readonly JsonSerializerOptions Compact = new() { Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping };
    readonly string root = config["DATA_DIR"] ?? "/data";

    public int Save(string collection, string id, JsonElement doc)
    {
        Directory.CreateDirectory(Path.Combine(root, collection));
        var path = Path.Combine(root, collection, id + ".jsonl");
        File.AppendAllText(path, JsonSerializer.Serialize(doc, Compact) + "\n");
        return File.ReadLines(path).Count();
    }

    public string? Get(string collection, string id)
    {
        var path = Path.Combine(root, collection, id + ".jsonl");
        return File.Exists(path) ? File.ReadLines(path).Last() : null;
    }

    public int Count(string collection, string id)
    {
        var path = Path.Combine(root, collection, id + ".jsonl");
        return File.Exists(path) ? File.ReadLines(path).Count() : 0;
    }

    public IEnumerable<string> List(string collection)
    {
        var dir = Path.Combine(root, collection);
        return !Directory.Exists(dir) ? [] : Directory.GetFiles(dir, "*.jsonl").Select(Path.GetFileNameWithoutExtension).Order(StringComparer.Ordinal)!;
    }
}
