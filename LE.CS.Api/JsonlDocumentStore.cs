using System.Collections.Concurrent;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace LE.CS.Api;

public sealed class JsonlDocumentStore(IConfiguration config) : IDocumentStore
{
    static readonly JsonSerializerOptions Compact = new() { Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping };
    readonly string root = config["DATA_DIR"] ?? "/data";
    readonly ConcurrentDictionary<string, SemaphoreSlim> locks = new();

    public int Save(string collection, string id, JsonElement doc)
    {
        Directory.CreateDirectory(Path.Combine(root, collection));
        var path = Path.Combine(root, collection, id + ".jsonl");
        var gate = locks.GetOrAdd(path, _ => new SemaphoreSlim(1, 1));
        gate.Wait();
        try
        {
            File.AppendAllText(path, JsonSerializer.Serialize(doc, Compact) + "\n");
            return File.ReadLines(path).Count();
        }
        finally
        {
            gate.Release();
        }
    }

    public string? Get(string collection, string id)
    {
        var path = Path.Combine(root, collection, id + ".jsonl");
        return File.Exists(path) ? File.ReadLines(path).Last() : null;
    }

    public string? Get(string collection, string id, int version)
    {
        var path = Path.Combine(root, collection, id + ".jsonl");
        return File.Exists(path) ? File.ReadLines(path).Skip(version - 1).FirstOrDefault() : null;
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
