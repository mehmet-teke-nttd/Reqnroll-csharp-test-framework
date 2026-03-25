namespace TestFramework.Common.Configuration;

public sealed class TestSettings
{
    public string BaseUrl { get; set; } = string.Empty;
    public string Browser { get; set; } = "chromium";
    public bool Headless { get; set; } = true;
    public int DefaultTimeoutMs { get; set; } = 30000;
    public string ArtifactsPath { get; set; } = "artifacts";
    public Dictionary<string, TestUser> Users { get; set; } = new();
}

public sealed class TestUser
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}