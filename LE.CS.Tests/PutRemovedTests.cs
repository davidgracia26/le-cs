using System.Net;
using System.Text;

namespace LE.CS.Tests;

public class PutRemovedTests
{
    [Fact]
    public async Task Put_Returns405()
    {
        using var api = new ApiFixture();
        var res = await api.CreateClient().PutAsync("/books/1", new StringContent("""{"title":"Dune"}""", Encoding.UTF8, "application/json"));
        Assert.Equal(HttpStatusCode.MethodNotAllowed, res.StatusCode);
    }

    [Fact]
    public async Task Put_On_Existing_Returns405_And_Adds_No_Version()
    {
        using var api = new ApiFixture();
        var client = api.CreateClient();
        await client.PostJson("/books/1", """{"v":1}""");
        await client.PostJson("/books/1", """{"v":2}""");
        var res = await client.PutAsync("/books/1", new StringContent("""{"v":3}""", Encoding.UTF8, "application/json"));
        Assert.Equal(HttpStatusCode.MethodNotAllowed, res.StatusCode);
        Assert.Equal("[1,2]", await client.GetStringAsync("/books/1/versions"));
    }
}
