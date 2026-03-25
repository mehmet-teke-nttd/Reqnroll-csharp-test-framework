using Microsoft.Playwright;

namespace TestFramework.Core.Session;

public sealed class UiScenarioSession
{
    public IBrowserContext? BrowserContext { get; set; }
    public IPage? Page { get; set; }
    public string? TracePath { get; set; }
    public string? ScreenshotPath { get; set; }
    
}