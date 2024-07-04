using OWP.Models;
using OWP.UserManagement.Helpers;
using Xunit;

namespace OWP.UserManagement.UnitTests.Helper;

public class ExportHelperUnitTest
{
    [Fact]
    public void BuildCorrectDataToExport_ShouldConvertValuesCorrectly()
    {
        // Arrange
        var users = new List<User>
        {
            new() { Pickup = "True", IsSecondAccount = "False" },
            new() { Pickup = "false", IsSecondAccount = "TRUE" },
            new() { Pickup = "unknown", IsSecondAccount = "something else" }
        };

        var expectedUsers = new List<User>
        {
            new() { Pickup = "yes", IsSecondAccount = "no" },
            new() { Pickup = "no", IsSecondAccount = "yes" },
            new() { Pickup = "none", IsSecondAccount = "none" }
        };

        // Act
        ExportHelper.BuildCorrectDataToExport(users);

        // Assert
        for (int i = 0; i < users.Count; i++)
        {
            Assert.Equal(expectedUsers[i].Pickup, users[i].Pickup);
            Assert.Equal(expectedUsers[i].IsSecondAccount, users[i].IsSecondAccount);
        }
    }
}