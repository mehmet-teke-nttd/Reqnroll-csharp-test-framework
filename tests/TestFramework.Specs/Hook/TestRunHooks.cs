using System;
using System.IO;
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

        var artifactsPath = Environment.GetEnvironmentVariable("ARTIFACTS_PATH") ?? "artifacts";
        var reportFolder = Path.GetFullPath(artifactsPath);
        var reportFile = Path.Combine(reportFolder, "ExtentReport.html");

        ExtentManager.Initialize(reportFile);

        Log.Information("Starting test run with TEST_ENV={Environment}. Extent report at {ReportFile}", environment, reportFile);
    }

    [AfterTestRun]
    public static void AfterTestRun()
    {
        ExtentManager.Flush();
        Log.CloseAndFlush();

        Log.Information("Test run complete. Extent report flushed.");
    }
}