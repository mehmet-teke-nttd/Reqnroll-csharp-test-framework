using Reqnroll;
using Serilog;

namespace TestFramework.Specs.Hooks;

[Binding]
public sealed class TestRunHooks
{
    [BeforeTestRun]
    public static void BeforeTestRun()
    {
        var environment = Environment.GetEnvironmentVariable("TEST_ENV")?.Trim().ToLowerInvariant();

        if (string.IsNullOrWhiteSpace(environment))
        {
            environment = "qa";
        }

        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .CreateLogger();

        Log.Information("Starting test run with TEST_ENV={Environment}", environment);
    }

    [AfterTestRun]
    public static void AfterTestRun()
    {
        Log.CloseAndFlush();
    }
}