using Microsoft.Playwright;
using TestFramework.Common.Configuration;

namespace TestFramework.Core.Drivers;

public sealed class BrowserManager : IAsyncDisposable
{
    private readonly TestSettings _settings;
    private IPlaywright? _playwright;
    private IBrowser? _browser;

    public BrowserManager(TestSettings settings)
    {
        _settings = settings;
    }

    public async Task<IBrowserContext> CreateContextAsync()
    {
        _playwright ??= await Playwright.CreateAsync();

        _browser ??= _settings.Browser.ToLowerInvariant() switch
        {
            "firefox" => await _playwright.Firefox.LaunchAsync(new BrowserTypeLaunchOptions
            {
                Headless = _settings.Headless
            }),
            "webkit" => await _playwright.Webkit.LaunchAsync(new BrowserTypeLaunchOptions
            {
                Headless = _settings.Headless
            }),
            _ => await _playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
            {
                Headless = _settings.Headless
            })
        };

        return await _browser.NewContextAsync(new BrowserNewContextOptions());
    }

    public async ValueTask DisposeAsync()
    {
        if (_browser is not null)
        {
            await _browser.CloseAsync();
        }

        _playwright?.Dispose();
    }
}