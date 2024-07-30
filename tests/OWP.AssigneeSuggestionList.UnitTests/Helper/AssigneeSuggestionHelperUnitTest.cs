using OWP.AssigneeSuggestionList.Helper;
using Xunit;

namespace OWP.AssigneeSuggestionList.UnitTests.Helper;

using Xunit.Asserts.Compare;

public class AssigneeSuggestionHelperUnitTest
{
    [Fact]
    public void Build_GetValidProperties_ReturnExpect()
    {
        var eventTime = new DateTime(2023, 3, 28, 2, 33, 00);
        var fromTime = new DateTime(2023, 3, 28, 1, 33, 00);
        var toTime = new DateTime(2023, 3, 28, 3, 33, 00);

        var result = PropertyScheduleHelper.GetValidProperties(eventTime, fromTime, toTime);

        DeepAssert.Equal(["Yesterday_02h", "Yesterday_03h"], result);
    }
}

