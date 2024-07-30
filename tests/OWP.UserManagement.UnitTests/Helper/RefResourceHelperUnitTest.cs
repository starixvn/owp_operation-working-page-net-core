using OWP.Models;
using OWP.UserManagement.Helpers;
using Xunit;
using Xunit.Asserts.Compare;

namespace OWP.UserManagement.UnitTests.Helper
{
    public class RefResourceHelperUnitTest
    {
        [Fact]
        public void SortUserGroups_ShouldSortGroupsCorrectly()
        {
            // Arrange
            var groups = new List<ReferenceResource>
            {
                new ReferenceResource { ResourceId = 1, Value = "Non-Existent Group", Name = "Group 1" },
                new ReferenceResource { ResourceId = 2, Value = "SUPPORT TEAM", Name = "Group 2" }
            };

            var expectedOrder = new List<ReferenceResource>
            {
                new ReferenceResource { ResourceId = 1, Value = "Non-Existent Group", Name = "Group 1" },
                new ReferenceResource { ResourceId = 2, Value = "SUPPORT TEAM", Name = "Group 2" }
            };

            // Act
            var sortedGroups = RefResourceHelper.SortUserGroups(groups);

            // Assert
            DeepAssert.Equal(expectedOrder, sortedGroups);
        }
    }
}