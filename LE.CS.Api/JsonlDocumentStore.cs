using System.Collections.Concurrent;
using System.Text.Json;

namespace LE.CS.Api;

public sealed class JsonlDocumentStore : IDocumentStore
{
    readonly ConcurrentDictionary<string, string> docs = new();

    public int Save(string collection, string id, JsonElement doc)
    {
        docs[$"{collection}/{id}"] = doc.GetRawText();
        return 1;
    }

    public string? Get(string collection, string id) => docs.GetValueOrDefault($"{collection}/{id}");
}
