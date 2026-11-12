using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;

namespace LE.CS.Tests;

public sealed class ApiFixture(string? dataDir = null) : WebApplicationFactory<Program>
{
    public string DataDir { get; } = dataDir ?? Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());

    protected override void ConfigureWebHost(IWebHostBuilder builder) => builder.UseSetting("DATA_DIR", DataDir);

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);
        if (disposing && Directory.Exists(DataDir)) Directory.Delete(DataDir, true);
    }
}
