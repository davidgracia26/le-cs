using Microsoft.AspNetCore.Mvc.Testing;

namespace LE.CS.Tests;

public class SmokeTests
{
    [Fact]
    public void Factory_Starts_Host()
    {
        var dir = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
        using var factory = new WebApplicationFactory<Program>().WithWebHostBuilder(b => b.UseSetting("DATA_DIR", dir));
        using var client = factory.CreateClient();
        Assert.NotNull(client);
    }
}
