using System.Text;

namespace LE.CS.Tests;

public static class Helpers
{
    public static Task<HttpResponseMessage> PostJson(this HttpClient client, string path, string body) =>
        client.PostAsync(path, new StringContent(body, Encoding.UTF8, "application/json"));
}
