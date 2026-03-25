using Microsoft.Extensions.DependencyInjection;
using Reqnroll.Microsoft.Extensions.DependencyInjection;
using TestFramework.Common.Configuration;
using TestFramework.Core.Drivers;
using TestFramework.Core.Session;

namespace TestFramework.Specs.Support;

public static class DependencyConfig
{
    [ScenarioDependencies]
    public static IServiceCollection CreateServices()
    {
        var services = new ServiceCollection();

        var settings = ConfigurationManager.Load();

        services.AddSingleton(settings);
        services.AddSingleton<TestUserResolver>();
        services.AddSingleton<BrowserManager>();

        services.AddScoped<UiScenarioSession>();

        return services;
    }
}