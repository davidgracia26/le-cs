using System.Net;
using System.Text;

namespace LE.CS.Tests;

public class SaveTests
{
    [Fact]
    public async Task Put_Returns_Version_1()
    {
        using var api = new ApiFixture();
        var res = await api.CreateClient().PutAsync("/books/1", new StringContent("""{"title":"Dune"}""", Encoding.UTF8, "application/json"));
        Assert.Equal(HttpStatusCode.OK, res.StatusCode);
        Assert.Equal("""{"version":1}""", await res.Content.ReadAsStringAsync());
    }

    [Fact]
    public async Task Put_TextPlain_Returns200()
    {
        using var api = new ApiFixture();
        var res = await api.CreateClient().PutAsync("/books/1", new StringContent("""{"title":"Dune"}""", Encoding.UTF8, "text/plain"));
        Assert.Equal(HttpStatusCode.OK, res.StatusCode);
    }

    [Fact]
    public async Task Put_NoContentType_Returns200()
    {
        using var api = new ApiFixture();
        var content = new ByteArrayContent(Encoding.UTF8.GetBytes("""{"title":"Dune"}"""));
        Assert.Null(content.Headers.ContentType);
        var res = await api.CreateClient().PutAsync("/books/1", content);
        Assert.Equal(HttpStatusCode.OK, res.StatusCode);
    }

    [Theory]
    [InlineData("[1,2]")]
    [InlineData("\"text\"")]
    [InlineData("42")]
    [InlineData("null")]
    public async Task Put_NonObject_Roundtrips(string body)
    {
        using var api = new ApiFixture();
        var client = api.CreateClient();
        var put = await client.PutJson("/books/1", body);
        Assert.Equal(HttpStatusCode.OK, put.StatusCode);
        Assert.Equal(body, await client.GetStringAsync("/books/1"));
    }
}
