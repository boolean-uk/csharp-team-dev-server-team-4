using Microsoft.AspNetCore.Mvc.Testing;

namespace exercise.wwwapi.Endpoints;

public static class TestUtils
{
    public static HttpClient CreateClient()
    {
        var factory = new WebApplicationFactory<Program>().WithWebHostBuilder(builder =>
        {
            builder.UseSetting("testing", "true");
        });
        return factory.CreateClient();
    }
}