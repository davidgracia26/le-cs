using System.Net;

namespace LE.CS.Tests;

public class LeftoverTests
{
    [Fact]
    public async Task WeatherForecast_Returns404()
    {
        using var api = new ApiFixture();
        var res = await api.CreateClient().GetAsync("/weatherforecast");
        Assert.Equal(HttpStatusCode.NotFound, res.StatusCode);
    }
}
