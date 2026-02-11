# Technical Debt Report

**Date**: 2026-02-11
**Analyzed Scope**: AIASD-20260209-BF

## Executive Summary
The `PostHubAPI` codebase demonstrates a standard N-tier architecture but suffers from significant neglect in maintenance and quality assurance. While the target framework (`net8.0`) is within support windows, the dependency tree is stale, and critical security practices (secrets management) are ignored. The complete absence of an automated testing suite makes any remediation efforts high-risk.

## 1. Explicit Debt (TODOs/FIXMEs)
| Type | File | Line | Description |
|------|------|------|-------------|
| - | - | - | No explicit technical debt tags (`TODO`, `FIXME`) were found in the codebase. |

## 2. Architectural Debt
- **Data Persistence**: `Program.cs` forces the use of `UseInMemoryDatabase("PostHubApi.db")`. This makes the API stateless across restarts, which is unsuitable for any real-world scenario.
- **Observability**: There is no configuring or usage of structured logging (`ILogger`). The application is opaque in production.
- **Security**:
  - **Secrets Management**: The JWT secret is hardcoded in `appsettings.json`.
  - **Dependencies**: `BCrypt.Net` is used at version `0.1.0`, which is likely deprecated or insecure compared to modern implementations like `BCrypt.Net-Next`.

## 3. Code Quality Issues
- **Async Implementation**: As identified in previous analysis, `UserController` has unawaited tasks, leading to broken authentication responses.
- **Magic Strings**: Configuration keys like `"JWT:Secret"` are accessed via string literals in `Program.cs`.
- **Exception Handling**: The application relies on global exception handling or default framework behaviors rather than explicit, granular handling in controllers.

## 4. Test Coverage
- **Status**: **Missing**
- **Findings**:
  - No test projects exist in the solution.
  - Critical paths like `UserService` and `UserController` are completely unverified.

## 5. Dependency Status
- **Framework**: `net8.0` (LTS). *Status: Supported (Ends Nov 2026).*
- **Outdated Packages**:
  - `Microsoft.AspNetCore.*` packages are pinned at `8.0.0`/`8.0.1`. These are likely missing 2+ years of security patches (current date: Feb 2026).
  - `BCrypt.Net` (`0.1.0`) is a major concern.
  - `Swashbuckle.AspNetCore` (`6.5.0`) is likely outdated.

## 6. Recommendations
1.  **[High] Initialize Test Suite**: Create a `PostHubAPI.Tests` project and add basic characterization tests before refactoring.
2.  **[High] Update Dependencies**:
    -   Update Microsoft packages to the latest `8.0.x` patch versions.
    -   Replace `BCrypt.Net` `0.1.0` with a maintained alternative.
3.  **[High] Fix Secrets**: User User Secrets for development and environment variables for production. Remove the secret from `appsettings.json`.
4.  **[Medium] Implement Logging**: Inject `ILogger` into services and document standard logging patterns.
5.  **[Medium] Architecture**: Plan migration to SQL Server or PostgreSQL (via containerization) to replace In-Memory DB.
