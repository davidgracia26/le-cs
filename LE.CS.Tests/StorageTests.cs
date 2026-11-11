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
}
