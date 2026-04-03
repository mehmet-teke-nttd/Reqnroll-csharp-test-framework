using System;
using System.IO;
using System.Threading;
using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using AventStack.ExtentReports.Reporter.Config;

namespace TestFramework.Specs.Hooks;

internal static class ExtentManager
{
    private static readonly object _lock = new();
    private static ExtentReports? _extent;
    private static readonly AsyncLocal<ExtentTest?> _testCurrent = new();

    public static void Initialize(string reportFilePath)
    {
        if (string.IsNullOrWhiteSpace(reportFilePath))
            throw new ArgumentException("Report file path cannot be null or whitespace.", nameof(reportFilePath));

        lock (_lock)
        {
            var outputDir = Path.GetDirectoryName(reportFilePath);
            if (string.IsNullOrWhiteSpace(outputDir))
                outputDir = ".";

            Directory.CreateDirectory(outputDir);

            _extent?.Flush();

            var sparkReporter = new ExtentSparkReporter(reportFilePath);
            sparkReporter.Config.DocumentTitle = "Test Execution Report";
            sparkReporter.Config.ReportName = "Test Execution Report";
            sparkReporter.Config.Theme = Theme.Dark;

            _extent = new ExtentReports();
            _extent.AttachReporter(sparkReporter);
            _extent.AddSystemInfo("User", Environment.UserName);
            _extent.AddSystemInfo("GeneratedOn", DateTime.UtcNow.ToString("O"));
        }
    }

    public static void Flush()
    {
        lock (_lock)
        {
            if (_extent is null)
                throw new InvalidOperationException("ExtentReports has not been initialized.");

            _extent.Flush();
        }
    }

    public static void CreateTest(string testName)
    {
        if (string.IsNullOrWhiteSpace(testName))
            throw new ArgumentException("Test name cannot be null or whitespace.", nameof(testName));

        lock (_lock)
        {
            if (_extent is null)
                throw new InvalidOperationException("ExtentReports has not been initialized. Call Initialize first.");

            _testCurrent.Value = _extent.CreateTest(testName);
        }
    }

    public static ExtentTest CurrentTest =>
        _testCurrent.Value ?? throw new InvalidOperationException("No current test has been created for this async context.");
}