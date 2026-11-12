using System.Net;

namespace LE.CS.Tests;

public class FetchTests
{
    [Fact]
    public async Task Get_Returns_Saved_Document()
    {
        using var api = new ApiFixture();
        var client = api.CreateClient();
        await client.PostJson("/books/1", """{"title":"Dune"}""");
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
        await client.PostJson("/books/1", """{"s":"é<&>日本"}""");
        var body = await client.GetStringAsync("/books/1");
        Assert.Equal("""{"s":"é<&>日本"}""", body);
        Assert.DoesNotContain("\\u", body);
    }

    [Fact]
    public async Task Get_Missing_Returns404()
    {
        using var api = new ApiFixture();
        var res = await api.CreateClient().GetAsync("/books/missing");
        Assert.Equal(HttpStatusCode.NotFound, res.StatusCode);
    }

    [Fact]
    public async Task Get_Keeps_Key_Order()
    {
        using var api = new ApiFixture();
        var client = api.CreateClient();
        await client.PostJson("/books/1", """{"b":1,"a":2}""");
        Assert.Equal("""{"b":1,"a":2}""", await client.GetStringAsync("/books/1"));
    }
}
