using System.Net;

namespace LE.CS.Tests;

public class ValidationTests
{
    [Fact]
    public async Task Put_InvalidJson_Returns400()
    {
        using var api = new ApiFixture();
        var res = await api.CreateClient().PutJson("/books/1", "{bad");
        Assert.Equal(HttpStatusCode.BadRequest, res.StatusCode);
    }

    [Fact]
    public async Task Put_EmptyBody_Returns400()
    {
        using var api = new ApiFixture();
        var res = await api.CreateClient().PutJson("/books/1", "");
        Assert.Equal(HttpStatusCode.BadRequest, res.StatusCode);
    }

    [Fact]
    public async Task Put_BadCollection_Returns400()
    {
        using var api = new ApiFixture();
        var res = await api.CreateClient().PutJson("/bad.name/1", "{}");
        Assert.Equal(HttpStatusCode.BadRequest, res.StatusCode);
    }

    [Fact]
    public async Task Put_BadId_Returns400()
    {
        using var api = new ApiFixture();
        var res = await api.CreateClient().PutJson("/books/bad.id", "{}");
        Assert.Equal(HttpStatusCode.BadRequest, res.StatusCode);
    }

    [Fact]
    public async Task List_BadCollection_Returns400()
    {
        using var api = new ApiFixture();
        var res = await api.CreateClient().GetAsync("/bad.name");
        Assert.Equal(HttpStatusCode.BadRequest, res.StatusCode);
    }

    [Fact]
    public async Task Get_BadCollection_Returns400()
    {
        using var api = new ApiFixture();
        var res = await api.CreateClient().GetAsync("/bad.name/1");
        Assert.Equal(HttpStatusCode.BadRequest, res.StatusCode);
    }

    [Fact]
    public async Task Put_ValidChars_Returns200()
    {
        using var api = new ApiFixture();
        var res = await api.CreateClient().PutJson("/Books_2/doc-1", "{}");
        Assert.Equal(HttpStatusCode.OK, res.StatusCode);
    }

    [Fact]
    public async Task Put_Id100_Returns200()
    {
        using var api = new ApiFixture();
        var res = await api.CreateClient().PutJson($"/books/{new string('a', 100)}", "{}");
        Assert.Equal(HttpStatusCode.OK, res.StatusCode);
    }

    [Fact]
    public async Task Put_Id101_Returns400()
    {
        using var api = new ApiFixture();
        var res = await api.CreateClient().PutJson($"/books/{new string('a', 101)}", "{}");
        Assert.Equal(HttpStatusCode.BadRequest, res.StatusCode);
    }
}
