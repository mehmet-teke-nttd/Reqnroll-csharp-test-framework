using FluentAssertions;
using Microsoft.Playwright;
using TestFramework.Common.Configuration;

namespace TestFramework.UI.Pages;

public sealed class LoginPage : BasePage
{
    private ILocator UsernameInput => Page.GetByPlaceholder("Username");
    private ILocator PasswordInput => Page.GetByPlaceholder("Password");
    private ILocator LoginButton => Page.GetByRole(AriaRole.Button, new() { Name = "Login" });
    private ILocator ErrorMessage => Page.GetByText("Epic sadface", new() { Exact = false });

    public LoginPage(IPage page, TestSettings settings) : base(page, settings)
    {
    }

    public async Task OpenAsync()
    {
        await Page.GotoAsync(Settings.BaseUrl);
    }

    public async Task LoginAsync(string username, string password)
    {
        await FillAsync(UsernameInput, username);
        await FillAsync(PasswordInput, password);
        await ClickAsync(LoginButton);
    }

    public async Task VerifyInventoryPageAsync()
    {
        await WaitForUrlAsync("**/inventory.html");
        Page.Url.Should().Contain("inventory.html");
    }

    public async Task VerifyLoginErrorAsync()
    {
        await ErrorMessage.WaitForAsync();

        var errorText = await ErrorMessage.InnerTextAsync();
        errorText.Should().Contain("Epic sadface");
    }
}