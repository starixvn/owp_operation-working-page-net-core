# OWP.NewLeagueSetting Unit Tests

This project contains comprehensive unit tests for the `OWP.NewLeagueSetting` project, covering all handlers, queries, services, endpoints, and criteria classes.

## Test Coverage

### Command Handlers

- **InsertLeagueSettingHandler** - Tests account creation functionality
- **UpdateLeagueSettingHandler** - Tests league setting update functionality
- **RemoveLeagueSettingHandler** - Tests league setting removal functionality

### Query Handlers

- **QueryLeagueSettingHandler** - Tests league setting query functionality
- **QueryLeagueSettingDetailHandler** - Tests league setting detail query functionality

### Services

- **DuplicateSettingService** - Tests duplicate detection logic and condition comparison

### Endpoints

- **NewLeagueSettingEndPoint** - Tests endpoint registration and route configuration

### Criteria

- **UpdateLeagueSettingCriteria** - Tests update criteria validation and properties
- **InsertLeagueSettingCriteria** - Tests insert criteria validation and properties
- **RemoveSettingCriteria** - Tests remove criteria validation and properties

## Test Organization

```
OWP.NewLeagueSetting.UnitTests/
├── InsertLeagueSettingHandlerUnitTest.cs      # Insert handler tests
├── UpdateLeagueSettingHandlerUnitTest.cs      # Update handler tests
├── RemoveLeagueSettingHandlerUnitTest.cs      # Remove handler tests
├── QueryLeagueSettingHandlerUnitTest.cs       # Query handler tests
├── QueryLeagueSettingDetailHandlerUnitTest.cs # Detail query handler tests
├── Service/                                   # Service tests
│   └── DuplicateSettingServiceUnitTest.cs    # Duplicate service tests
├── EndPoints/                                 # Endpoint tests
│   └── NewLeagueSettingEndPointUnitTest.cs   # Endpoint registration tests
├── Criteria/                                  # Criteria tests
│   ├── UpdateLeagueSettingCriteriaUnitTest.cs
│   ├── InsertLeagueSettingCriteriaUnitTest.cs
│   └── RemoveSettingCriteriaUnitTest.cs
└── README.md                                  # This file
```

## Test Scenarios Covered

### InsertLeagueSettingHandler

- ✅ Successful league setting insertion
- ✅ Repository method calls with correct parameters
- ✅ AutoMapper integration
- ✅ Session user ID handling
- ✅ Different user ID scenarios
- ✅ Edge case user IDs (zero, negative)

### UpdateLeagueSettingHandler

- ✅ Successful league setting updates
- ✅ StandardizedLeagueInfo method calls
- ✅ Repository update method calls
- ✅ Session user ID handling
- ✅ Different user ID scenarios
- ✅ Edge case user IDs (zero, negative)

### RemoveLeagueSettingHandler

- ✅ Successful league setting removal
- ✅ RemoveSettingCriteria creation
- ✅ Repository ExecuteNonQueryCommand calls
- ✅ Session user ID handling
- ✅ Different league setting IDs
- ✅ Edge case IDs (zero, negative)

### QueryLeagueSettingHandler

- ✅ Successful league setting queries
- ✅ Repository query method calls
- ✅ Different request parameters
- ✅ Empty and null request handling
- ✅ Large result sets
- ✅ Cancellation token handling

### QueryLeagueSettingDetailHandler

- ✅ Successful detail queries
- ✅ Repository detail query method calls
- ✅ Different league setting IDs
- ✅ Edge case IDs (zero, negative, large)
- ✅ Null result handling
- ✅ Cancellation token handling

### DuplicateSettingService

- ✅ Duplicate detection logic
- ✅ No duplicate scenarios
- ✅ Exact duplicate scenarios
- ✅ Same ID exclusion
- ✅ Different condition scenarios
- ✅ Empty result sets
- ✅ Condition comparison methods
- ✅ Edge case condition lists

### NewLeagueSettingEndPoint

- ✅ All endpoint registrations
- ✅ Route group creation
- ✅ Authorization requirements
- ✅ Anonymous endpoint handling
- ✅ Middleware filter integration
- ✅ HTTP method mappings
- ✅ Content type specifications

### Criteria Classes

- ✅ Setting key generation
- ✅ Validation logic
- ✅ Property access and setters
- ✅ Inheritance verification
- ✅ Default value handling
- ✅ Edge case validation

## Running the Tests

### Prerequisites

- .NET 9.0 SDK
- Visual Studio 2022 or VS Code with C# extension

### Command Line

```bash
# Navigate to test project directory
cd tests/OWP.NewLeagueSetting.UnitTests

# Run all tests
dotnet test

# Run tests with verbose output
dotnet test --verbosity normal

# Run tests with coverage
dotnet test --collect:"XPlat Code Coverage"

# Run specific test class
dotnet test --filter "FullyQualifiedName~UpdateLeagueSettingHandlerUnitTest"

# Run specific test method
dotnet test --filter "FullyQualifiedName~Handle_ShouldBuildStandardizedInfo_AndCallRepository_AndReturnUnit"
```

### Visual Studio

1. Open the solution file
2. Right-click on `OWP.NewLeagueSetting.UnitTests` project
3. Select "Run Tests"

### VS Code

1. Install .NET Core Test Explorer extension
2. Open the test project
3. Use the Test Explorer panel to run tests

## Test Dependencies

- **xUnit** - Testing framework
- **NSubstitute** - Mocking framework
- **Microsoft.NET.Test.Sdk** - Test SDK

## Mocking Strategy

The tests use **NSubstitute** for mocking dependencies:

- **Repository** - Mocked to verify method calls and return test data
- **INapSession** - Mocked to return test user session data
- **IMapper** - Mocked to return test mapped objects
- **IEndpointRouteBuilder** - Mocked to verify endpoint registration
- **IEndpointConventionBuilder** - Mocked to verify endpoint configuration

## Test Data Management

Test data is created inline within each test method to ensure:

- **Isolation** - Each test has its own data
- **Clarity** - Test data is visible and understandable
- **Maintainability** - Easy to modify test scenarios
- **Realism** - Data represents realistic business scenarios

## Best Practices

1. **Arrange-Act-Assert Pattern** - All tests follow the AAA pattern
2. **Descriptive Test Names** - Test methods clearly describe the scenario being tested
3. **Comprehensive Coverage** - Tests cover success, failure, and edge cases
4. **Mock Verification** - Tests verify that dependencies are called correctly
5. **Edge Case Testing** - Tests include boundary conditions and unusual scenarios
6. **Property Testing** - Tests verify all public properties work correctly
7. **Inheritance Testing** - Tests verify proper class inheritance

## Adding New Tests

When adding new functionality to the NewLeagueSetting project:

1. **Create Handler Tests** - Test all public methods and edge cases
2. **Create Query Tests** - Test query logic and repository calls
3. **Create Service Tests** - Test business logic and algorithms
4. **Create Endpoint Tests** - Test route registration and configuration
5. **Create Criteria Tests** - Test validation logic and properties
6. **Follow Naming Convention** - Use descriptive test method names
7. **Test Exception Scenarios** - Ensure proper error handling
8. **Verify Dependencies** - Check that dependencies are called correctly

## Test Categories

- **Unit Tests** - Test individual components in isolation
- **Integration Tests** - Test component interactions
- **Edge Case Tests** - Test boundary conditions and unusual scenarios
- **Property Tests** - Test object properties and validation
- **Mock Verification Tests** - Verify dependency interactions

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
3. **Use Descriptive Names** - Test methods should clearly describe what they test
4. **Document Complex Tests** - Add comments for complex test scenarios
5. **Keep Tests Focused** - Each test should verify one specific behavior
6. **Test Edge Cases** - Include boundary conditions and unusual scenarios

## Test Metrics

- **Total Test Count**: 50+ tests
- **Coverage Areas**: Handlers, Queries, Services, Endpoints, Criteria
- **Test Categories**: Unit Tests, Integration Tests, Edge Case Tests
- **Mock Usage**: Comprehensive dependency mocking
- **Assertion Types**: Equality, Exception, Mock verification, Property verification

## Performance Considerations

- **Fast Execution** - All tests should run quickly (< 100ms each)
- **No External Dependencies** - Tests don't hit databases or external services
- **Efficient Mocking** - Minimal overhead from mock setup
- **Parallel Execution** - Tests can run in parallel for faster execution

## Code Coverage Goals

- **Handlers**: 100% method coverage
- **Services**: 100% method coverage
- **Criteria**: 100% property and method coverage
- **Endpoints**: 100% route registration coverage
- **Overall**: >95% line coverage
