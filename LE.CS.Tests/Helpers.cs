using System.Text;

namespace LE.CS.Tests;

public static class Helpers
{
    public static Task<HttpResponseMessage> PutJson(this HttpClient client, string path, string body) =>
        client.PutAsync(path, new StringContent(body, Encoding.UTF8, "application/json"));
}
