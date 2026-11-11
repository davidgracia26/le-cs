using System.Text.Json;

namespace LE.CS.Api;

public sealed class JsonlDocumentStore : IDocumentStore
{
    public int Save(string collection, string id, JsonElement doc) => 1;
}
