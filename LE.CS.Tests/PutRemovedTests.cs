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
}
