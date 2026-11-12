using System.Net;
using System.Text.Json;

namespace LE.CS.Tests;

public class ConcurrencyTests
{
    [Fact]
    public async Task Concurrent_Puts_Keep_All_Versions()
    {
        using var api = new ApiFixture();
        var client = api.CreateClient();
        var sent = Enumerable.Range(1, 50).Select(i => $$"""{"n":{{i}}}""").ToList();
        var responses = await Task.WhenAll(sent.Select(body => Task.Run(() => client.PutJson("/books/1", body))));
        Assert.All(responses, r => Assert.Equal(HttpStatusCode.OK, r.StatusCode));
        Assert.Equal(Enumerable.Range(1, 50), JsonSerializer.Deserialize<int[]>(await client.GetStringAsync("/books/1/versions")));
        var stored = new List<string>();
        for (var n = 1; n <= 50; n++) stored.Add(await client.GetStringAsync($"/books/1/versions/{n}"));
        Assert.Equal(sent.Order(), stored.Order());
    }
}
