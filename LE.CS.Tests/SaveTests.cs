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
}
