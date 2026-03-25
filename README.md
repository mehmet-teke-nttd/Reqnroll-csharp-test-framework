# Reqnroll C# Test Framework

A scalable Behavior-Driven Development (BDD) test automation framework built with C#, Reqnroll, Playwright, and NUnit.

---

## Overview

This framework provides a clean, maintainable, and extensible structure for UI and API test automation using modern .NET practices.

Key features:

* BDD with Reqnroll
* UI automation with Playwright
* NUnit test execution
* Environment-based configuration
* Dependency Injection (DI)
* Secure credential handling via environment variables
* HTML reporting
* Screenshot and trace capture on failure

---

## Tech Stack

* .NET 8/9
* Reqnroll
* NUnit
* Microsoft Playwright
* FluentAssertions
* Microsoft.Extensions.Configuration
* Microsoft.Extensions.DependencyInjection
* Serilog

---

## Project Structure

```
TestFramework.sln
│
├── src/
│   ├── TestFramework.Common      # Configuration, models, utilities
│   ├── TestFramework.Core        # Drivers, session management
│   ├── TestFramework.UI          # Page objects
│   └── TestFramework.API         # API layer (future)
│
└── tests/
    └── TestFramework.Specs
        ├── Features              # .feature files
        ├── Steps                 # Step definitions
        ├── Hooks                 # Test lifecycle hooks
        ├── Support               # DI configuration
        └── appsettings*.json     # Environment configs
```

---

## Setup

### 1. Clone the repository

```
git clone https://github.com/mehmet-teke-nttd/Reqnroll-csharp-test-framework.git
cd Reqnroll-csharp-test-framework
```

### 2. Install dependencies

```
dotnet restore
```

### 3. Install Playwright browsers

```
playwright install
```

---

## Running Tests

### Option 1: Using script (recommended)

```
.\scripts\local-run-tests.ps1
```

### Option 2: Manual execution

PowerShell:

```
$env:TEST_ENV="dev"
$env:TESTSETTINGS__USERS__STANDARD__PASSWORD="secret_sauce"
dotnet test
```

---

## Environment Configuration

The framework supports multiple environments:

* `appsettings.json` (base)
* `appsettings.dev.json`
* `appsettings.qa.json`

Environment is selected using:

```
TEST_ENV=dev
```

Configuration loading order:

1. appsettings.json
2. appsettings.{env}.json
3. Environment variables

---

## Secrets Management

Credentials are NOT stored in feature files.

Passwords are injected via environment variables:

```
TESTSETTINGS__USERS__STANDARD__PASSWORD
```

Example:

```
$env:TESTSETTINGS__USERS__STANDARD__PASSWORD="secret_sauce"
```

---

## Writing Tests

### Example Feature

```
Scenario: Successful login
  Given the user opens the login page
  When the "standard" user logs in
  Then the inventory page should be displayed
```

### Step Definitions

* Use DI for dependencies
* Keep steps thin
* Delegate logic to page objects

---

## Page Object Model

* All locators and actions are in page classes
* No selectors in step definitions
* Common logic lives in `BasePage`

---

## Test Artifacts

Generated under `artifacts/`:

* `screenshots/` – on failure
* `traces/` – Playwright traces

View trace:

```
playwright show-trace <trace-file>.zip
```

---

## Reporting

HTML report is generated after test execution:

```
bin/Debug/net*/report/reqnroll_report.html
```

---

## Best Practices

* Do not hardcode test data in steps
* Keep feature files business-readable
* Use DI instead of static access
* Keep page objects focused on behavior
* Capture diagnostics for failures

---

## Future Improvements

* API automation layer
* Allure reporting integration
* Parallel execution
* CI/CD pipeline integration
* Test data builders

---

## Author

GitHub: [https://github.com/mehmet-teke-nttd](https://github.com/mehmet-teke-nttd)

---

## License

This project is for learning and demonstration purposes.
