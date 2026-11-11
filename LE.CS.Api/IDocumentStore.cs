using System.Text.Json;

namespace LE.CS.Api;

public interface IDocumentStore
{
    int Save(string collection, string id, JsonElement doc);
}
