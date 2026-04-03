using System;
using System.IO;
using System.Linq;
using AventStack.ExtentReports;
using Microsoft.Playwright;
using Reqnroll;
using TestFramework.Common.Configuration;
using TestFramework.Core.Drivers;
using TestFramework.Core.Session;

namespace TestFramework.Specs.Hooks;

[Binding]
public sealed class ScenarioHooks
{
    private readonly BrowserManager _browserManager;
    private readonly ScenarioContext _scenarioContext;
    private readonly UiScenarioSession _uiSession;
    private readonly TestSettings _settings;

    public ScenarioHooks(
        BrowserManager browserManager,
        ScenarioContext scenarioContext,
        UiScenarioSession uiSession,
        TestSettings settings)
    {
        _browserManager = browserManager;
        _scenarioContext = scenarioContext;
        _uiSession = uiSession;
        _settings = settings;
    }

    [BeforeScenario]
    public async Task BeforeScenarioAsync()
    {
        var scenarioName = _scenarioContext.ScenarioInfo.Title;
        ExtentManager.CreateTest(scenarioName);

        if (_scenarioContext.ScenarioInfo.Tags?.Any() == true)
        {
            foreach (var tag in _scenarioContext.ScenarioInfo.Tags)
            {
                ExtentManager.CurrentTest?.AssignCategory(tag.TrimStart('@'));
            }
        }

        ExtentManager.CurrentTest?.Info($"Starting scenario: {scenarioName}");

        _uiSession.BrowserContext = await _browserManager.CreateContextAsync();

        await _uiSession.BrowserContext.Tracing.StartAsync(new TracingStartOptions
        {
            Screenshots = true,
            Snapshots = true,
            Sources = true
        });

        _uiSession.Page = await _uiSession.BrowserContext.NewPageAsync();
        _uiSession.Page.SetDefaultTimeout(_settings.DefaultTimeoutMs);
    }

    [AfterScenario]
    public async Task AfterScenarioAsync()
    {
        var screenshotsPath = Path.Combine(_settings.ArtifactsPath, "screenshots");
        var tracesPath = Path.Combine(_settings.ArtifactsPath, "traces");

        Directory.CreateDirectory(screenshotsPath);
        Directory.CreateDirectory(tracesPath);

        var safeScenarioTitle = SanitizeFileName(_scenarioContext.ScenarioInfo.Title);
        var timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");

        if (_scenarioContext.TestError is not null)
        {
            if (_uiSession.Page is not null)
            {
                _uiSession.ScreenshotPath = Path.Combine(
                    screenshotsPath,
                    $"{safeScenarioTitle}_{timestamp}.png");

                await _uiSession.Page.ScreenshotAsync(new PageScreenshotOptions
                {
                    Path = _uiSession.ScreenshotPath,
                    FullPage = true
                });
            }

            if (_uiSession.BrowserContext is not null)
            {
                _uiSession.TracePath = Path.Combine(
                    tracesPath,
                    $"{safeScenarioTitle}_{timestamp}.zip");

                await _uiSession.BrowserContext.Tracing.StopAsync(new TracingStopOptions
                {
                    Path = _uiSession.TracePath
                });
            }
        }
        else
        {
            if (_uiSession.BrowserContext is not null)
            {
                await _uiSession.BrowserContext.Tracing.StopAsync();
            }
        }

        if (_uiSession.Page is not null)
        {
            await _uiSession.Page.CloseAsync();
        }

        var extentTest = ExtentManager.CurrentTest;

        if (_scenarioContext.TestError is not null)
        {
            extentTest?.Fail(_scenarioContext.TestError);

            if (!string.IsNullOrEmpty(_uiSession.ScreenshotPath) && File.Exists(_uiSession.ScreenshotPath))
            {
                extentTest?.AddScreenCaptureFromPath(_uiSession.ScreenshotPath, "Screenshot on failure");
            }

            if (!string.IsNullOrEmpty(_uiSession.TracePath) && File.Exists(_uiSession.TracePath))
            {
                extentTest?.Info($"Trace file: {_uiSession.TracePath}");
            }
        }
        else
        {
            extentTest?.Pass("Scenario passed");
        }

        if (_uiSession.BrowserContext is not null)
        {
            await _uiSession.BrowserContext.CloseAsync();
        }
    }

    private static string SanitizeFileName(string name)
    {
        foreach (var invalidChar in Path.GetInvalidFileNameChars())
        {
            name = name.Replace(invalidChar, '_');
        }

        return name;
    }
}