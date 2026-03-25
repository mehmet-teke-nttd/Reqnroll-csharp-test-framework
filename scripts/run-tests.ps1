param(
    [ValidateSet("qa", "uat")]
    [string]$Environment = "qa"
)

$ErrorActionPreference = "Stop"

$env:TEST_ENV = $Environment

switch ($Environment) {
    "qa" {
        if (-not $env:TESTSETTINGS__USERS__STANDARD__PASSWORD) {
            $env:TESTSETTINGS__USERS__STANDARD__PASSWORD = "secret_sauce"
        }

        if (-not $env:TESTSETTINGS__USERS__INVALID__PASSWORD) {
            $env:TESTSETTINGS__USERS__INVALID__PASSWORD = "wrong_password"
        }
    }
    "uat" {
        if (-not $env:TESTSETTINGS__USERS__STANDARD__PASSWORD) {
            throw "Missing environment variable TESTSETTINGS__USERS__STANDARD__PASSWORD for uat."
        }

        if (-not $env:TESTSETTINGS__USERS__INVALID__PASSWORD) {
            throw "Missing environment variable TESTSETTINGS__USERS__INVALID__PASSWORD for uat."
        }
    }
}

Write-Host "Running tests with TEST_ENV=$Environment"
dotnet test