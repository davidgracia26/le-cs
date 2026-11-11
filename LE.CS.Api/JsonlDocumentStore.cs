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
        File.AppendAllText(Path.Combine(root, collection, id + ".jsonl"), JsonSerializer.Serialize(doc, Compact) + "\n");
        return 1;
    }

    public string? Get(string collection, string id)
    {
        var path = Path.Combine(root, collection, id + ".jsonl");
        return File.Exists(path) ? File.ReadLines(path).Last() : null;
    }
}
