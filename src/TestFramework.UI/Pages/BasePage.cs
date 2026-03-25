using Microsoft.Playwright;
using TestFramework.Common.Configuration;

namespace TestFramework.UI.Pages;

public abstract class BasePage
{
    protected readonly IPage Page;
    protected readonly TestSettings Settings;

    protected BasePage(IPage page, TestSettings settings)
    {
        Page = page;
        Settings = settings;
    }

    protected async Task ClickAsync(ILocator locator)
    {
        await locator.ClickAsync();
    }

    protected async Task FillAsync(ILocator locator, string value)
    {
        await locator.FillAsync(value);
    }

    protected async Task WaitForUrlAsync(string urlPattern)
    {
        await Page.WaitForURLAsync(urlPattern);
    }

    protected async Task<bool> IsVisibleAsync(ILocator locator)
    {
        return await locator.IsVisibleAsync();
    }
}