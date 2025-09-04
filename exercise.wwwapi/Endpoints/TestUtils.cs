using Microsoft.AspNetCore.Mvc.Testing;

namespace exercise.wwwapi.Endpoints;

public static class TestUtils
{
    public static HttpClient CreateClient()
    {
        var factory = new WebApplicationFactory<Program>().WithWebHostBuilder(builder =>
        {
            builder.UseSetting(Globals.TestingEnvVariable, "true");
        });
        return factory.CreateClient();
    }
}