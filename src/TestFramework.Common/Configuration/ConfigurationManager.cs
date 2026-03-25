using Microsoft.Extensions.Configuration;
using System.IO;

namespace TestFramework.Common.Configuration;

public static class ConfigurationManager
{
    public static TestSettings Load()
    {
        var environment = Environment.GetEnvironmentVariable("TEST_ENV")?.Trim().ToLowerInvariant();

        if (string.IsNullOrWhiteSpace(environment))
        {
            environment = "qa";
        }

        var configuration = new ConfigurationBuilder()
            .SetBasePath(Path.Combine(AppContext.BaseDirectory, "..", "..", ".."))
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
            .AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: false)
            .AddEnvironmentVariables()
            .Build();

        var settings = new TestSettings();
        configuration.GetSection("TestSettings").Bind(settings);

        Validate(settings, environment);

        return settings;
    }

    private static void Validate(TestSettings settings, string environment)
    {
        if (string.IsNullOrWhiteSpace(settings.BaseUrl))
        {
            throw new InvalidOperationException(
                $"TestSettings:BaseUrl is missing for environment '{environment}'.");
        }

        if (string.IsNullOrWhiteSpace(settings.Browser))
        {
            throw new InvalidOperationException(
                $"TestSettings:Browser is missing for environment '{environment}'.");
        }

        if (settings.DefaultTimeoutMs <= 0)
        {
            throw new InvalidOperationException(
                $"TestSettings:DefaultTimeoutMs must be greater than zero for environment '{environment}'.");
        }

        if (string.IsNullOrWhiteSpace(settings.ArtifactsPath))
        {
            throw new InvalidOperationException(
                $"TestSettings:ArtifactsPath is missing for environment '{environment}'.");
        }
    }
}