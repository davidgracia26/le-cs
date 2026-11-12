using System.Net;

namespace LE.CS.Tests;

public class VersionTests
{
    [Fact]
    public async Task Second_Post_Returns_Version_2()
    {
        using var api = new ApiFixture();
        var client = api.CreateClient();
        await client.PostJson("/books/1", """{"version":1}""");
        var res = await client.PostJson("/books/1", """{"title":"Other"}""");
        Assert.Equal(HttpStatusCode.OK, res.StatusCode);
        Assert.Equal("""{"version":2}""", await res.Content.ReadAsStringAsync());
    }

    [Fact]
    public async Task Get_Returns_Second_Body()
    {
        using var api = new ApiFixture();
        var client = api.CreateClient();
        await client.PostJson("/books/1", """{"v":1}""");
        await client.PostJson("/books/1", """{"v":2}""");
        Assert.Equal("""{"v":2}""", await client.GetStringAsync("/books/1"));
    }

    [Fact]
    public async Task Versions_Lists_1_2_3()
    {
        using var api = new ApiFixture();
        var client = api.CreateClient();
        foreach (var body in new[] { "1", "2", "3" }) await client.PostJson("/books/1", body);
        var res = await client.GetAsync("/books/1/versions");
        Assert.Equal(HttpStatusCode.OK, res.StatusCode);
        Assert.Equal("application/json", res.Content.Headers.ContentType?.MediaType);
        Assert.Equal("[1,2,3]", await res.Content.ReadAsStringAsync());
    }

    [Fact]
    public async Task Version_1_Returns_First_Body()
    {
        using var api = new ApiFixture();
        var client = api.CreateClient();
        await client.PostJson("/books/1", """{"v":1}""");
        await client.PostJson("/books/1", """{"v":2}""");
        var res = await client.GetAsync("/books/1/versions/1");
        Assert.Equal(HttpStatusCode.OK, res.StatusCode);
        Assert.Equal("application/json", res.Content.Headers.ContentType?.MediaType);
        Assert.Equal("""{"v":1}""", await res.Content.ReadAsStringAsync());
    }

    [Fact]
    public async Task Version_2_Returns_Second_Body()
    {
        using var api = new ApiFixture();
        var client = api.CreateClient();
        await client.PostJson("/books/1", """{"v":1}""");
        await client.PostJson("/books/1", """{"v":2}""");
        var res = await client.GetAsync("/books/1/versions/2");
        Assert.Equal(HttpStatusCode.OK, res.StatusCode);
        Assert.Equal("""{"v":2}""", await res.Content.ReadAsStringAsync());
    }

    [Fact]
    public async Task Identical_Post_Adds_Version()
    {
        using var api = new ApiFixture();
        var client = api.CreateClient();
        await client.PostJson("/books/1", """{"v":1}""");
        var res = await client.PostJson("/books/1", """{"v":1}""");
        Assert.Equal(HttpStatusCode.OK, res.StatusCode);
        Assert.Equal("[1,2]", await client.GetStringAsync("/books/1/versions"));
    }

    [Fact]
    public async Task Version_Beyond_Last_Returns404()
    {
        using var api = new ApiFixture();
        var client = api.CreateClient();
        foreach (var body in new[] { "1", "2", "3" }) await client.PostJson("/books/1", body);
        var res = await client.GetAsync("/books/1/versions/9");
        Assert.Equal(HttpStatusCode.NotFound, res.StatusCode);
    }

    [Fact]
    public async Task Version_0_Returns404()
    {
        using var api = new ApiFixture();
        var client = api.CreateClient();
        await client.PostJson("/books/1", """{"v":1}""");
        var res = await client.GetAsync("/books/1/versions/0");
        Assert.Equal(HttpStatusCode.NotFound, res.StatusCode);
    }

    [Fact]
    public async Task Version_Abc_Returns404()
    {
        using var api = new ApiFixture();
        var client = api.CreateClient();
        await client.PostJson("/books/1", """{"v":1}""");
        var res = await client.GetAsync("/books/1/versions/abc");
        Assert.Equal(HttpStatusCode.NotFound, res.StatusCode);
    }

    [Fact]
    public async Task Versions_Of_Missing_Returns404()
    {
        using var api = new ApiFixture();
        var res = await api.CreateClient().GetAsync("/books/missing/versions");
        Assert.Equal(HttpStatusCode.NotFound, res.StatusCode);
    }

    [Fact]
    public async Task Version_Of_Missing_Returns404()
    {
        using var api = new ApiFixture();
        var res = await api.CreateClient().GetAsync("/books/missing/versions/1");
        Assert.Equal(HttpStatusCode.NotFound, res.StatusCode);
    }

    [Fact]
    public async Task Post_Leaves_Other_Documents_Unchanged()
    {
        using var api = new ApiFixture();
        var client = api.CreateClient();
        await client.PostJson("/books/1", """{"v":1}""");
        await client.PostJson("/books/2", """{"v":1}""");
        await client.PostJson("/books/1", """{"v":2}""");
        Assert.Equal("[1,2]", await client.GetStringAsync("/books/1/versions"));
        Assert.Equal("[1]", await client.GetStringAsync("/books/2/versions"));
    }

    [Fact]
    public async Task Failed_Post_Adds_No_Version()
    {
        using var api = new ApiFixture();
        var client = api.CreateClient();
        await client.PostJson("/books/1", """{"v":1}""");
        var bad = await client.PostJson("/books/1", "{bad");
        Assert.Equal(HttpStatusCode.BadRequest, bad.StatusCode);
        Assert.Equal("[1]", await client.GetStringAsync("/books/1/versions"));
    }
}
