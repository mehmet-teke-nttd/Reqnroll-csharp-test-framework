namespace TestFramework.Common.Configuration;

public sealed class TestUserResolver
{
    private readonly TestSettings _settings;

    public TestUserResolver(TestSettings settings)
    {
        _settings = settings;
    }

    public TestUser GetUser(string userKey)
    {
        if (string.IsNullOrWhiteSpace(userKey))
        {
            throw new ArgumentException("User key cannot be null or empty.", nameof(userKey));
        }

        var normalizedUserKey = userKey.Trim();

        if (!_settings.Users.TryGetValue(normalizedUserKey, out var configuredUser) || configuredUser is null)
        {
            throw new KeyNotFoundException($"No configured test user found for key '{normalizedUserKey}'.");
        }

        var username = configuredUser.Username?.Trim();
        if (string.IsNullOrWhiteSpace(username))
        {
            throw new InvalidOperationException($"Username is missing for user '{normalizedUserKey}'.");
        }

        var password = GetPasswordFromEnvironment(normalizedUserKey);

        if (string.IsNullOrWhiteSpace(password))
        {
            password = configuredUser.Password?.Trim();
        }

        if (string.IsNullOrWhiteSpace(password))
        {
            throw new InvalidOperationException(
                $"Password is missing for user '{normalizedUserKey}'. " +
                $"Set it in config or environment variable TESTSETTINGS__USERS__{normalizedUserKey.ToUpperInvariant()}__PASSWORD.");
        }

        return new TestUser
        {
            Username = username,
            Password = password
        };
    }

    private static string? GetPasswordFromEnvironment(string userKey)
    {
        var envVarName = $"TESTSETTINGS__USERS__{userKey.ToUpperInvariant()}__PASSWORD";
        return Environment.GetEnvironmentVariable(envVarName);
    }
}