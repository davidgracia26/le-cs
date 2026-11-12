using System.Net;

namespace LE.CS.Tests;

public class ListTests
{
    [Fact]
    public async Task List_Returns_Sorted_Ids()
    {
        using var api = new ApiFixture();
        var client = api.CreateClient();
        await client.PostJson("/books/2", "{}");
        await client.PostJson("/books/1", "{}");
        var res = await client.GetAsync("/books");
        Assert.Equal(HttpStatusCode.OK, res.StatusCode);
        Assert.Equal("application/json", res.Content.Headers.ContentType?.MediaType);
        Assert.Equal("""["1","2"]""", await res.Content.ReadAsStringAsync());
    }

    [Fact]
    public async Task List_Empty_Collection_Returns_EmptyArray()
    {
        using var api = new ApiFixture();
        var res = await api.CreateClient().GetAsync("/nothing");
        Assert.Equal(HttpStatusCode.OK, res.StatusCode);
        Assert.Equal("[]", await res.Content.ReadAsStringAsync());
    }
}
