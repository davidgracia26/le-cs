using System.Net;

namespace LE.CS.Tests;

public class FetchTests
{
    [Fact]
    public async Task Get_Returns_Saved_Document()
    {
        using var api = new ApiFixture();
        var client = api.CreateClient();
        await client.PutJson("/books/1", """{"title":"Dune"}""");
        var res = await client.GetAsync("/books/1");
        Assert.Equal(HttpStatusCode.OK, res.StatusCode);
        Assert.Equal("application/json", res.Content.Headers.ContentType?.MediaType);
        Assert.Equal("""{"title":"Dune"}""", await res.Content.ReadAsStringAsync());
    }

    [Fact]
    public async Task Get_Keeps_Unicode_Unescaped()
    {
        using var api = new ApiFixture();
        var client = api.CreateClient();
        await client.PutJson("/books/1", """{"s":"é<&>日本"}""");
        var body = await client.GetStringAsync("/books/1");
        Assert.Equal("""{"s":"é<&>日本"}""", body);
        Assert.DoesNotContain("\\u", body);
    }
}
