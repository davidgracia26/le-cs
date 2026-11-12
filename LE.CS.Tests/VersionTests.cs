using System.Net;

namespace LE.CS.Tests;

public class VersionTests
{
    [Fact]
    public async Task Second_Put_Returns_Version_2()
    {
        using var api = new ApiFixture();
        var client = api.CreateClient();
        await client.PutJson("/books/1", """{"version":1}""");
        var res = await client.PutJson("/books/1", """{"title":"Other"}""");
        Assert.Equal(HttpStatusCode.OK, res.StatusCode);
        Assert.Equal("""{"version":2}""", await res.Content.ReadAsStringAsync());
    }
}
