using System.Net;

namespace LE.CS.Tests;

public class ValidationTests
{
    [Fact]
    public async Task Post_InvalidJson_Returns400()
    {
        using var api = new ApiFixture();
        var res = await api.CreateClient().PostJson("/books/1", "{bad");
        Assert.Equal(HttpStatusCode.BadRequest, res.StatusCode);
    }

    [Fact]
    public async Task Post_EmptyBody_Returns400()
    {
        using var api = new ApiFixture();
        var res = await api.CreateClient().PostJson("/books/1", "");
        Assert.Equal(HttpStatusCode.BadRequest, res.StatusCode);
    }

    [Fact]
    public async Task Post_BadCollection_Returns400()
    {
        using var api = new ApiFixture();
        var res = await api.CreateClient().PostJson("/bad.name/1", "{}");
        Assert.Equal(HttpStatusCode.BadRequest, res.StatusCode);
    }

    [Fact]
    public async Task Post_BadId_Returns400()
    {
        using var api = new ApiFixture();
        var res = await api.CreateClient().PostJson("/books/bad.id", "{}");
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
    public async Task Versions_BadCollection_Returns400()
    {
        using var api = new ApiFixture();
        var res = await api.CreateClient().GetAsync("/bad.name/1/versions");
        Assert.Equal(HttpStatusCode.BadRequest, res.StatusCode);
    }

    [Fact]
    public async Task Version_BadId_Returns400()
    {
        using var api = new ApiFixture();
        var res = await api.CreateClient().GetAsync("/books/bad.id/versions/1");
        Assert.Equal(HttpStatusCode.BadRequest, res.StatusCode);
    }

    [Fact]
    public async Task Post_ValidChars_Returns200()
    {
        using var api = new ApiFixture();
        var res = await api.CreateClient().PostJson("/Books_2/doc-1", "{}");
        Assert.Equal(HttpStatusCode.OK, res.StatusCode);
    }

    [Fact]
    public async Task Post_Id100_Returns200()
    {
        using var api = new ApiFixture();
        var res = await api.CreateClient().PostJson($"/books/{new string('a', 100)}", "{}");
        Assert.Equal(HttpStatusCode.OK, res.StatusCode);
    }

    [Fact]
    public async Task Post_Id101_Returns400()
    {
        using var api = new ApiFixture();
        var res = await api.CreateClient().PostJson($"/books/{new string('a', 101)}", "{}");
        Assert.Equal(HttpStatusCode.BadRequest, res.StatusCode);
    }

    [Fact]
    public async Task Post_Collection100_Returns200()
    {
        using var api = new ApiFixture();
        var res = await api.CreateClient().PostJson($"/{new string('a', 100)}/1", "{}");
        Assert.Equal(HttpStatusCode.OK, res.StatusCode);
    }

    [Fact]
    public async Task Post_Collection101_Returns400()
    {
        using var api = new ApiFixture();
        var res = await api.CreateClient().PostJson($"/{new string('a', 101)}/1", "{}");
        Assert.Equal(HttpStatusCode.BadRequest, res.StatusCode);
    }
}
