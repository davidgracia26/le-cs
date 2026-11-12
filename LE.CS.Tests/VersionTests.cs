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

    [Fact]
    public async Task Get_Returns_Second_Body()
    {
        using var api = new ApiFixture();
        var client = api.CreateClient();
        await client.PutJson("/books/1", """{"v":1}""");
        await client.PutJson("/books/1", """{"v":2}""");
        Assert.Equal("""{"v":2}""", await client.GetStringAsync("/books/1"));
    }

    [Fact]
    public async Task Versions_Lists_1_2_3()
    {
        using var api = new ApiFixture();
        var client = api.CreateClient();
        foreach (var body in new[] { "1", "2", "3" }) await client.PutJson("/books/1", body);
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
        await client.PutJson("/books/1", """{"v":1}""");
        await client.PutJson("/books/1", """{"v":2}""");
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
        await client.PutJson("/books/1", """{"v":1}""");
        await client.PutJson("/books/1", """{"v":2}""");
        var res = await client.GetAsync("/books/1/versions/2");
        Assert.Equal(HttpStatusCode.OK, res.StatusCode);
        Assert.Equal("""{"v":2}""", await res.Content.ReadAsStringAsync());
    }

    [Fact]
    public async Task Post_On_Existing_Returns_Version_2()
    {
        using var api = new ApiFixture();
        var client = api.CreateClient();
        await client.PutJson("/books/1", """{"v":1}""");
        var res = await client.PostAsync("/books/1", new StringContent("""{"v":2}""", System.Text.Encoding.UTF8, "application/json"));
        Assert.Equal(HttpStatusCode.OK, res.StatusCode);
        Assert.Equal("""{"version":2}""", await res.Content.ReadAsStringAsync());
    }

    [Fact]
    public async Task Identical_Put_Adds_Version()
    {
        using var api = new ApiFixture();
        var client = api.CreateClient();
        await client.PutJson("/books/1", """{"v":1}""");
        var res = await client.PutJson("/books/1", """{"v":1}""");
        Assert.Equal(HttpStatusCode.OK, res.StatusCode);
        Assert.Equal("[1,2]", await client.GetStringAsync("/books/1/versions"));
    }
}
