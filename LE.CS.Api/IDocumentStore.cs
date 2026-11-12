using System.Text.Json;

namespace LE.CS.Api;

public interface IDocumentStore
{
    int Save(string collection, string id, JsonElement doc);
    string? Get(string collection, string id);
    string? Get(string collection, string id, int version);
    int Count(string collection, string id);
    IEnumerable<string> List(string collection);
}
