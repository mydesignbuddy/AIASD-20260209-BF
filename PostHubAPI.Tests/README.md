# PostHubAPI.Tests

This is the unit test project for the PostHubAPI application.

## Test Framework

- **XUnit** 2.9.3 - Testing framework
- **Moq** 4.20.72 - Mocking framework for dependencies
- **Microsoft.AspNetCore.Mvc.Testing** 8.0.0 - ASP.NET Core testing utilities

## Running Tests

### Command Line
```bash
# Run all tests
dotnet test

# Run tests with detailed output
dotnet test --logger:"console;verbosity=detailed"

# Run tests in specific project
dotnet test PostHubAPI.Tests/PostHubAPI.Tests.csproj
```

### Visual Studio
- Open Test Explorer (Test > Test Explorer)
- Click "Run All" to execute all tests
- Individual tests can be run by right-clicking

### VS Code
- Install the .NET Test Explorer extension
- Tests will appear in the Test Explorer panel
- Click the play button to run tests

## Test Structure

### UserServiceTests
Tests for the `UserService` class covering authentication functionality:

- **Register_WithValidData_ReturnsToken** - Verifies successful user registration
- **Register_WithExistingEmail_ThrowsArgumentException** - Tests duplicate email handling
- **Register_WithFailedCreation_ThrowsArgumentException** - Tests registration failure scenarios
- **Login_WithValidCredentials_ReturnsToken** - Verifies successful login
- **Login_WithNonExistentUser_ThrowsArgumentException** - Tests login with invalid username
- **Login_WithInvalidPassword_ThrowsArgumentException** - Tests login with wrong password

### UserControllerTests
Tests for the `UserController` API endpoints:

- **Register_WithValidModel_ReturnsOkResult** - Tests successful registration endpoint
- **Register_WithInvalidModelState_ReturnsBadRequest** - Tests model validation
- **Register_WithServiceException_ReturnsBadRequest** - Documents async/await bug
- **Login_WithValidModel_ReturnsOkResult** - Tests successful login endpoint
- **Login_WithInvalidModelState_ReturnsBadRequest** - Tests model validation
- **Login_WithServiceException_ReturnsBadRequest** - Documents async/await bug

## Known Issues Documented in Tests

The tests currently document an existing bug in the controller methods:
- Controller methods call async service methods without awaiting them
- This causes exceptions to not be caught properly
- Tests are written to verify current behavior (returning Task<string> instead of string)
- These tests serve as regression tests when the bug is fixed

## Test Coverage

Current coverage focuses on:
- ✅ User registration functionality
- ✅ User login functionality  
- ✅ Input validation
- ✅ Error handling
- ⚠️ Controller async/await behavior (documents bug)

## Adding New Tests

When adding new tests:
1. Follow the existing naming convention: `MethodName_Scenario_ExpectedResult`
2. Use the Arrange-Act-Assert pattern
3. Mock external dependencies using Moq
4. Include XML comments for complex scenarios
5. Group related tests in the same test class

Example:
```csharp
[Fact]
public void MethodName_WithValidInput_ReturnsExpectedResult()
{
    // Arrange
    var input = new SomeDto { /* ... */ };
    
    // Act
    var result = _service.MethodName(input);
    
    // Assert
    Assert.NotNull(result);
    Assert.Equal(expectedValue, result.Property);
}
```

## Dependencies

Tests depend on:
- PostHubAPI project (via project reference)
- Mocked UserManager and IConfiguration
- XUnit test attributes and assertions

## Contributing

When contributing tests:
1. Ensure all tests pass before committing
2. Add tests for new features
3. Update tests when fixing bugs
4. Maintain test coverage above 70%
5. Document any known issues in test comments
