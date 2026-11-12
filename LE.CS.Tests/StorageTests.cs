using System.Text.Json;
using LE.CS.Api;
using Microsoft.Extensions.Configuration;

namespace LE.CS.Tests;

public class StorageTests
{
    [Fact]
    public async Task Put_Writes_Jsonl_File()
    {
        using var api = new ApiFixture();
        await api.CreateClient().PutJson("/books/1", """{"title":"Dune"}""");
        var lines = File.ReadAllLines(Path.Combine(api.DataDir, "books", "1.jsonl"));
        Assert.Equal(["""{"title":"Dune"}"""], lines);
    }

    [Fact]
    public async Task Put_Compacts_Pretty_Body_To_One_Line()
    {
        using var api = new ApiFixture();
        await api.CreateClient().PutJson("/books/1", "{\n  \"a\": 1,\n  \"b\": [1, 2]\n}");
        var lines = File.ReadAllLines(Path.Combine(api.DataDir, "books", "1.jsonl"));
        Assert.Equal(["""{"a":1,"b":[1,2]}"""], lines);
    }

    [Fact]
    public async Task File_Has_One_Line_Per_Version()
    {
        using var api = new ApiFixture();
        var client = api.CreateClient();
        foreach (var body in new[] { "1", "2", "3" }) await client.PutJson("/books/1", body);
        var text = File.ReadAllText(Path.Combine(api.DataDir, "books", "1.jsonl"));
        Assert.Equal("1\n2\n3\n", text);
    }

    [Fact]
    public async Task Data_Dir_Setting_Sets_Write_Path()
    {
        using var api = new ApiFixture();
        await api.CreateClient().PutJson("/books/1", "{}");
        Assert.True(File.Exists(Path.Combine(api.DataDir, "books", "1.jsonl")));
    }

    [Fact]
    public void Unset_Data_Dir_Writes_Under_Slash_Data()
    {
        var collection = "t-" + Guid.NewGuid().ToString("N");
        try
        {
            new JsonlDocumentStore(new ConfigurationBuilder().Build()).Save(collection, "1", JsonSerializer.Deserialize<JsonElement>("{}"));
            Assert.True(File.Exists($"/data/{collection}/1.jsonl"));
        }
        finally
        {
            Directory.Delete($"/data/{collection}", true);
        }
    }

    [Fact]
    public async Task New_Instance_Reads_Prior_Instance_Data()
    {
        using var first = new ApiFixture();
        var a = first.CreateClient();
        await a.PutJson("/books/1", """{"v":1}""");
        await a.PutJson("/books/1", """{"v":2}""");
        using var second = new ApiFixture(first.DataDir);
        var b = second.CreateClient();
        Assert.Equal("""{"v":2}""", await b.GetStringAsync("/books/1"));
        Assert.Equal("[1,2]", await b.GetStringAsync("/books/1/versions"));
    }
}
