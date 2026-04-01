using Reqnroll;
using TestFramework.Common.Configuration;
using TestFramework.Core.Session;
using TestFramework.UI.Pages;

namespace TestFramework.Specs.Steps;

[Binding]
public sealed class LoginSteps
{
    private readonly LoginPage _loginPage;
    private readonly TestUserResolver _testUserResolver;

    public LoginSteps(
        UiScenarioSession uiSession,
        TestSettings settings,
        TestUserResolver testUserResolver)
    {
        if (uiSession.Page is null)
        {
            throw new InvalidOperationException("UI session page was not initialized.");
        }

        _loginPage = new LoginPage(uiSession.Page, settings);
        _testUserResolver = testUserResolver;
    }

  
        [Given(@"the user opens the login page")]
        public async Task Giventheuseropenstheloginpage()
        {
            await _loginPage.OpenAsync();
        }

        [When(@"the ""(.*)"" user logs in")]
        public async Task Whentheuserlogsin(string userKey)
        {
            var user = _testUserResolver.GetUser(userKey);
                await _loginPage.LoginAsync(user.Username, user.Password);
        }

        [Then(@"the inventory page should be displayed")]
        public async Task Thentheinventorypageshouldbedisplayed()
        {
            await _loginPage.VerifyInventoryPageAsync();
        }






   

    [When("the user logs in with username {string} and password {string}")]
    public async Task WhenTheUserLogsInWithUsernameAndPassword(string username, string password)
    {
        await _loginPage.LoginAsync(username, password);
    }

   

    [Then("a login error should be displayed")]
    public async Task ThenALoginErrorShouldBeDisplayed()
    {
        await _loginPage.VerifyLoginErrorAsync();
    }
}