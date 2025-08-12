# OWP.ChatServer Unit Tests

This project contains comprehensive unit tests for the `OWP.ChatServer` project, covering all handlers, endpoints, and criteria classes.

## Test Coverage

### Handlers

- **AccountValidatorHandler** - Tests account validation functionality
- **AccountCreatorHandler** - Tests account creation functionality
- **UpdatePasswordHandler** - Tests password update functionality
- **UpdateUserNameHandler** - Tests username update functionality
- **GetChatServerHandler** - Tests chat server info retrieval

### Endpoints

- **SyncUserEndpoint** - Tests user synchronization endpoint registration
- **GetChatServerInfoEndpoint** - Tests chat server info endpoint registration

### Criteria

- **MarkUserUpdatePasswordCriteria** - Tests password update criteria validation
- **UpdateChatUserInfoCriteria** - Tests chat user info update criteria validation

### Test Utilities

- **TestDataBuilder** - Helper class for creating test data and HTTP responses

## Test Organization

```
OWP.ChatServer.UnitTests/
├── Handler/                    # Handler class tests
│   ├── AccountValidatorHandlerTests.cs
│   ├── AccountCreatorHandlerTests.cs
│   ├── UpdatePasswordHandlerTests.cs
│   ├── UpdateUserNameHandlerTests.cs
│   └── GetChatServerHandlerTests.cs
├── Endpoint/                   # Endpoint class tests
│   ├── SyncUserEndpointTests.cs
│   └── GetChatServerInfoEndpointTests.cs
├── Criteria/                   # Criteria class tests
│   ├── MarkUserUpdatePasswordCriteriaTests.cs
│   └── UpdateChatUserInfoCriteriaTests.cs
├── TestUtilities/              # Test helper classes
│   └── TestDataBuilder.cs
└── README.md                   # This file
```

## Test Scenarios Covered

### AccountValidatorHandler

- ✅ User exists validation
- ✅ User does not exist validation
- ✅ HTTP request failures
- ✅ Exception handling
- ✅ Logging verification

### AccountCreatorHandler

- ✅ Successful account creation
- ✅ Failed account creation
- ✅ Exception handling
- ✅ Configuration handling
- ✅ Logging verification

### UpdatePasswordHandler

- ✅ Successful password update
- ✅ User not found scenarios
- ✅ Empty user ID handling
- ✅ Update failures
- ✅ Exception handling
- ✅ Logging verification

### UpdateUserNameHandler

- ✅ Single username update
- ✅ Multiple username updates
- ✅ User not found scenarios
- ✅ Partial failure handling
- ✅ Exception handling
- ✅ Logging verification

### GetChatServerHandler

- ✅ Configuration retrieval
- ✅ Missing configuration handling
- ✅ Empty configuration handling
- ✅ Object structure validation

### Criteria Classes

- ✅ Setting key validation
- ✅ Property access
- ✅ Interface implementation
- ✅ Validation logic

### Endpoint Classes

- ✅ Route registration
- ✅ Interface implementation
- ✅ Exception handling

## Running the Tests

### Prerequisites

- .NET 9.0 SDK
- Visual Studio 2022 or VS Code with C# extension

### Command Line

```bash
# Navigate to test project directory
cd tests/OWP.ChatServer.UnitTests

# Run all tests
dotnet test

# Run tests with verbose output
dotnet test --verbosity normal

# Run tests with coverage
dotnet test --collect:"XPlat Code Coverage"

# Run specific test class
dotnet test --filter "FullyQualifiedName~AccountValidatorHandlerTests"

# Run specific test method
dotnet test --filter "FullyQualifiedName~Handle_WhenUserExists_ReturnsTrue"
```

### Visual Studio

1. Open the solution file
2. Right-click on `OWP.ChatServer.UnitTests` project
3. Select "Run Tests"

### VS Code

1. Install .NET Core Test Explorer extension
2. Open the test project
3. Use the Test Explorer panel to run tests

## Test Dependencies

- **xUnit** - Testing framework
- **NSubstitute** - Mocking framework
- **Microsoft.NET.Test.Sdk** - Test SDK
- **Microsoft.AspNetCore.TestHost** - ASP.NET Core testing utilities

## Mocking Strategy

The tests use **NSubstitute** for mocking dependencies:

- **IHttpClientFactory** - Mocked to return test HttpClient instances
- **IDynamicRepository** - Mocked to verify logging calls
- **IConfiguration** - Mocked to return test configuration values
- **INapSession** - Mocked to return test user session data
- **HttpClient** - Mocked to return test HTTP responses

## Test Data Management

The `TestDataBuilder` utility class provides:

- **HTTP Responses** - Pre-built HTTP response objects for different scenarios
- **Request Objects** - Factory methods for creating test request objects
- **Test Constants** - Common test values used across multiple tests

## Best Practices

1. **Arrange-Act-Assert Pattern** - All tests follow the AAA pattern
2. **Descriptive Test Names** - Test methods clearly describe the scenario being tested
3. **Comprehensive Coverage** - Tests cover success, failure, and edge cases
4. **Mock Verification** - Tests verify that dependencies are called correctly
5. **Exception Testing** - Tests verify proper exception handling
6. **Logging Verification** - Tests verify that logging occurs as expected

## Adding New Tests

When adding new functionality to the ChatServer project:

1. **Create Handler Tests** - Test all public methods and edge cases
2. **Create Endpoint Tests** - Test route registration and basic functionality
3. **Create Criteria Tests** - Test validation logic and properties
4. **Update TestDataBuilder** - Add new test data creation methods if needed
5. **Follow Naming Convention** - Use descriptive test method names
6. **Test Exception Scenarios** - Ensure proper error handling
7. **Verify Logging** - Check that appropriate logging occurs

## Troubleshooting

### Common Issues

1. **Missing Dependencies** - Ensure all NuGet packages are restored
2. **Build Errors** - Check that the main project builds successfully
3. **Test Failures** - Review test output for detailed error information
4. **Mock Issues** - Verify that mocks are set up correctly

### Debugging Tests

1. **Set Breakpoints** - Use breakpoints in test methods
2. **Debug Output** - Use `Console.WriteLine` or debugger output
3. **Test Explorer** - Use Visual Studio Test Explorer for debugging
4. **Verbose Output** - Run tests with `--verbosity normal` for more details

## Contributing

When contributing to the test suite:

1. **Follow Existing Patterns** - Maintain consistency with existing tests
2. **Add Comprehensive Coverage** - Test all scenarios, not just happy path
3. **Use Test Utilities** - Leverage existing test data builders
4. **Document Complex Tests** - Add comments for complex test scenarios
5. **Keep Tests Focused** - Each test should verify one specific behavior

## Test Metrics

- **Total Test Count**: 50+ tests
- **Coverage Areas**: Handlers, Endpoints, Criteria, Utilities
- **Test Categories**: Unit Tests, Integration Tests, Exception Tests
- **Mock Usage**: Comprehensive dependency mocking
- **Assertion Types**: Equality, Exception, Mock verification, Logging verification
